using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Serialization.Tasks
{

    // TODO: Implement GoogleSearchResult class to be deserialized from Google Search API response
    // Specification is available at: https://developers.google.com/custom-search/v1/using_rest#WorkingResults
    // The test json file is at Serialization.Tests\Resources\GoogleSearchJson.txt

    public class GoogleSearchResult
    {    
        [DataMember(Name = "kind")]
        public string Kind;

        [DataMember(Name = "url")]
        public Url Url;

        [DataMember(Name = "queries")]
        public Queries Queries;

        [DataMember(Name = "context")]
        public Context Context;

        [DataMember(Name = "items")]
        public Item[] Items;
   }

    [DataContract]
    public class Url
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "template")]
        public string Template { get; set; }
    }

    [DataContract]
    public class Queries
    {
        [DataMember(Name = "nextPages")]
        public Page[] NextPages { get; set; }
        [DataMember(Name = "request")]
        public Page[] Request { get; set; }
    }

    [DataContract]
    public class Page
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "totalResults")]
        public int TotalResults { get; set; }

        [DataMember(Name = "searchTerms")]
        public string searchTerms { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "startIndex")]
        public int StartIndex { get; set; }

        [DataMember(Name = "inputEncoding")]
        public string InputEncoding { get; set; }

        [DataMember(Name = "outputEncoding")]
        public string OutputEncoding { get; set; }

        [DataMember(Name = "cx")]
        public string Cx { get; set; }
    }
    [DataContract]
    public class Context
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }

    [DataContract]
    public class Item
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "htmlTitle")]
        public string HtmlTitle { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "displayLink")]
        public string DisplayLink { get; set; }
        [DataMember(Name = "snippet")]
        public string Snippet { get; set; }
        [DataMember(Name = "htmlSnippet")]
        public string HtmlSnippet { get; set; }
        public Pagemap pagemap { get; set; }
    }
    [DataContract]
    public class Pagemap
    {
        public RTO[] RTO { get; set; }
    }
    [DataContract]
    public class RTO
    {
        [DataMember(Name = "format")]
        public string Format { get; set; }
        [DataMember(Name = "group_impression_tag")]
        public string GroupImpressionTag { get; set; }
        [DataMember(Name = "Optmax_rank_top")]
        public string OptmaxRankTop { get; set; }
        [DataMember(Name = "Optthreshold_override")]
        public string OptthresholdOverride { get; set; }
        [DataMember(Name = "Optdisallow_same_domain")]
        public string OptdisallowSameDomain { get; set; }
        [DataMember(Name = "Outputtitle")]
        public string Outputtitle { get; set; }
        [DataMember(Name = "Outputwant_title_on_right")]
        public string OutputwantTitleOnRight { get; set; }
        [DataMember(Name = "Outputnum_lines1")]
        public string OutputnumLines1 { get; set; }
        [DataMember(Name = "Outputtext1")]
        public string Outputtext1 { get; set; }
        [DataMember(Name = "Outputgray1b")]
        public string Outputgray1b { get; set; }
        [DataMember(Name = "Outputno_clip1b")]
        public string OutputnoClip1b { get; set; }
        [DataMember(Name = "UrlOutputurl2")]
        public string UrlOutputurl2 { get; set; }
        [DataMember(Name = "Outputlink2")]
        public string Outputlink2 { get; set; }
        [DataMember(Name = "Outputtext2b")]
        public string Outputtext2b { get; set; }
        [DataMember(Name = "UrlOutputurl2c")]
        public string UrlOutputurl2c { get; set; }
        [DataMember(Name = "Outputlink2c")]
        public string Outputlink2c { get; set; }
        [DataMember(Name = "result_group_header")]
        public string ResultGroupHeader { get; set; }
        [DataMember(Name = "Outputimage_url")]
        public string OutputimageUrl { get; set; }
        [DataMember(Name = "image_size")]
        public string ImageSize { get; set; }
        [DataMember(Name = "Outputinline_image_width")]
        public string OutputinlineImageWidth { get; set; }
        [DataMember(Name = "Outputinline_image_height")]
        public string OutputinlineImageHeight { get; set; }
        [DataMember(Name = "Outputimage_border")]
        public string OutputimageBorder { get; set; }

    }
}
