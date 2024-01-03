using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Globalization;

namespace KlioBlazor.Shared
{
    [XmlRoot(ElementName = "desc")]
    public class Desc
    {
        [XmlAttribute(AttributeName = "lang")]
        public string Lang { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "title")]
    public class Title
    {
        [XmlAttribute(AttributeName = "lang")]
        public string Lang { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "programme")]
    public class Programme
    {
        [XmlAttribute(AttributeName = "start")]
        public string Start { get; set; }
        [XmlAttribute(AttributeName = "stop")]
        public string Stop { get; set; }
        [XmlAttribute(AttributeName = "channel")]
        public string Channel { get; set; }
        [XmlElement(ElementName = "title")]
        public Title Title { get; set; }
        [XmlElement(ElementName = "desc")]
        public Desc Desc { get; set; }

        public DateTime StartDatetIme
        {
            get
            {
                return DateTime.ParseExact(Start, "yyyyMMddHHmmss zzz", DateTimeFormatInfo.CurrentInfo);
            }
        }

        public DateTime StopDatetIme
        {
            get
            {
                return DateTime.ParseExact(Stop, "yyyyMMddHHmmss zzz", DateTimeFormatInfo.CurrentInfo);
            }
        }

        public string TitleBrief
        {
            get
            {
                if (string.IsNullOrEmpty(Title.Text))
                {
                    return string.Empty;
                }

                if (Title.Text.Length > 36)
                {
                    return Title.Text.Substring(0, 36) + "...";
                }
                else
                {
                    return Title.Text;
                }
            }
        }
    }

    [XmlRoot(ElementName = "icon")]
    public class Icon
    {
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
    }

    [XmlRoot(ElementName = "display-name")]
    public class DisplayName
    {
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "channel")]
    public class Channel
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "title")]
        public List<DisplayName> DisplayNames { get; set; }
        [XmlElement(ElementName = "icon")]
        public Icon Icon { get; set; }
    }

    [XmlRoot(ElementName = "tv")]
    public class TV
    {
        [XmlAttribute(AttributeName = "generator-info-name")]
        public string GeneratorInfoName { get; set; }
        [XmlAttribute(AttributeName = "generator-info-url")]
        public string GeneratorInfoUrl { get; set; }
        [XmlElement(ElementName = "channel")]
        public List<Channel> Channels { get; set; }
        [XmlElement(ElementName = "programme")]
        public List<Programme> Programmes { get; set; }
    }
}
