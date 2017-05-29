#include "json/xmlwriter.h"
#include "json/writer.h"

namespace Json
{
	static string wrap(const string& tag, const string& value)
	{
		if (value.empty())
			return "<" + tag + "/>";
		else
			return "<" + tag + ">" + value + "</" + tag + ">";
	}

	static string escapeXml(const string& raw)
	{
		string value;

		string::size_type n = 0;
		while (true)
		{
			string::size_type pos  = raw.find_first_of("&<>\"\'", n);
			if (pos != n)
				value += raw.substr(n, pos - n);
			if (pos == string::npos)
				break;

			switch (raw[pos])
			{
				case '&': value += "&amp;"; break;
				case '<': value += "&lt;"; break;
				case '>': value += "&gt;"; break;
				case '\"': value += "&quot;"; break;
				case '\'': value += "&apos;"; break;
			}

			n = ++pos;
		}
		return value;
	}

	string XMLWriter::write(const Value& root, const string& tag)
	{
		m_doc = "";
		writeValue(root, tag);
		return m_doc;
	}

	void XMLWriter::writeValue(const Value& value, const string& tag)
	{
		switch(value.type())
		{
			case nullValue:
				m_doc += wrap(tag, "");
				break;
			case intValue:
				m_doc += wrap(tag, valueToString(value.asInt()));
				break;
			case uintValue:
				m_doc += wrap(tag, valueToString(value.asUInt()));
				break;
			case int64Value:
				m_doc += wrap(tag, valueToString(value.asInt64()));
				break;
			case uint64Value:
				m_doc += wrap(tag, valueToString(value.asUInt64()));
				break;
			case realValue:
				m_doc += wrap(tag, valueToString(value.asDouble()));
				break;
			case stringValue:
				m_doc += wrap(tag, escapeXml(value.asCString()));
				break;
			case booleanValue:
				m_doc += wrap(tag, valueToString(value.asBool()));
				break;
			case arrayValue:
				{
					string tag1, tag2;
					int pos = tag.find_last_of(":");
					if ((unsigned int)pos == string::npos) {
						tag1 = "";
						tag2 = tag;
					} else {
						tag1 = tag.substr(0, pos);
						tag2 = tag.substr(pos+1);
					}

					int size = value.size();
					if (size == 0)
					{
						m_doc += "<" + tag + "/>";
					}
					else
					{
						// find the suffix
						if (!tag1.empty())
							m_doc += "<" + tag1 + ">";
						for(int i=0; i<size; i++)
							writeValue(value[i], tag2); // TODO specific tag names for array elements
						if (!tag1.empty())
							m_doc += "</" + tag1 + ">";
					}
				}
				break;
			case objectValue:
				{
					Value::Members members(value.getMemberNames());
					if (members.empty())
					{
						m_doc += "<" + tag + "/>";
					}
					else
					{
						m_doc += "<" + tag + ">";
						for(Value::Members::iterator it=members.begin();
								it != members.end();
								++it)
						{
							const string& name = *it;
							writeValue(value[name], name);
						}
						m_doc += "</" + tag + ">";
					}
				}
				break;
		}
	}
}
