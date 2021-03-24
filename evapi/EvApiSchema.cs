using System;
using System.Collections.Generic;

// TODO: Implement automatic JSON serialization.

namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class EvResponseStatus : StringEnum
    {
        public EvResponseStatus(string value) : base(value)
        {
        }

        public static EvResponseStatus Success => new EvResponseStatus("success");
        public static EvResponseStatus Error => new EvResponseStatus("error");
    }


    public class EvExportRange : StringEnum
    {
        public EvExportRange(string value) : base(value)
        {
        }

        public static EvExportRange AllPages => new EvExportRange("all_pages");
        public static EvExportRange CurrentPage => new EvExportRange("current_page");
        public static EvExportRange SelectedObjects => new EvExportRange("selected_objects");
    }

    public class EvColorMode : StringEnum
    {
        public EvColorMode(string value) : base(value)
        {
        }

        public static EvColorMode Rgb => new EvColorMode("rgb");
        public static EvColorMode Grayscale => new EvColorMode("grayscale");
    }

    public class EvInterpolation : StringEnum
    {
        public EvInterpolation(string value) : base(value)
        {
        }

        public static EvInterpolation Lanczos => new EvInterpolation("lanczos");
        public static EvInterpolation Bilinear => new EvInterpolation("bilinear");
        public static EvInterpolation Triangle => new EvInterpolation("triangle");
        public static EvInterpolation Bell => new EvInterpolation("bell");
        public static EvInterpolation Bspline => new EvInterpolation("bspline");
        public static EvInterpolation Mitchell => new EvInterpolation("mitchell");
        public static EvInterpolation Bicubic => new EvInterpolation("bicubic");
    }

    public class EvResponseError : Exception
    {
        public int Code { get; }
        public string Text { get; }

        public EvResponseError(int code, string text) : base(CreateMessage(code, text))
        {
            Code = code;
            Text = text;
        }

        private static string CreateMessage(int code, string text)
        {
            string err = $"Envision response error {code}";
            if (text.Length > 0)
                err = $"{err}: {text}";
            return err;
        }
    }

    public class EvResponse
    {
        private readonly Dict _data;

        public EvResponse(Dict data)
        {
            _data = data;
        }

        public string GetRespType()
        {
            return (string) _data["type"];
        }

        public string GetCmd()
        {
            return (string) _data["cmd"];
        }

        public string GetStatus()
        {
            return (string) _data["status"];
        }

        public Dict GetOutput()
        {
            if (_data.ContainsKey("data"))
                return (Dict) _data["data"];
            return new Dict();
        }

        public EvResponseError GetError()
        {
            int code = (int) _data["err_code"];
            string text = (string) _data["err_text"];
            return new EvResponseError(code, text);
        }
    }

    public class EvDocument
    {
        private readonly Dict _data;

        public EvDocument(Dict data)
        {
            _data = data;
        }

        public int GetId()
        {
            return (int) _data["id"];
        }
    }

    public class EvPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public EvPoint(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }
    }

    public class EvSize
    {
        public int W { get; set; }
        public int H { get; set; }

        public EvSize(int w = 0, int h = 0)
        {
            W = w;
            H = h;
        }
    }

    public class EvExportOptions
    {
        public EvExportRange PageRange { get; set; }
        public bool SkipHiddenPages { get; set; }
        public bool UseObjectBounds { get; set; }
        public bool CreateFolder { get; set; }
        public int Resolution { get; set; }
        public EvColorMode ColorMode { get; set; }
        public EvInterpolation Interpolation { get; set; }
        public bool AntiAlias { get; set; }

        public EvExportOptions(
            EvExportRange pageRange = null,
            bool skipHiddenPages = true,
            bool useObjectBounds = false,
            bool createFolder = false,
            int resolution = 300,
            EvColorMode colorMode = null,
            EvInterpolation interpolation = null,
            bool antiAlias = false)
        {
            PageRange = pageRange ?? EvExportRange.AllPages;
            SkipHiddenPages = skipHiddenPages;
            UseObjectBounds = useObjectBounds;
            CreateFolder = createFolder;
            Resolution = resolution;
            ColorMode = colorMode ?? EvColorMode.Rgb;
            Interpolation = interpolation ?? EvInterpolation.Lanczos;
            AntiAlias = antiAlias;
        }
    }

    public class EvTesselationQuality : StringEnum
    {
        public EvTesselationQuality(string value) : base(value)
        {
        }

        public static EvTesselationQuality ExtraHigh => new EvTesselationQuality("extra_high");
        public static EvTesselationQuality High => new EvTesselationQuality("high");
        public static EvTesselationQuality Medium => new EvTesselationQuality("medium");
        public static EvTesselationQuality Low => new EvTesselationQuality("low");
        public static EvTesselationQuality ExtraLow => new EvTesselationQuality("extra_low");
    }

    public class EvUnit : StringEnum
    {
        public EvUnit(string value) : base(value)
        {
        }

        public static EvUnit In => new EvUnit("in");
        public static EvUnit Cm => new EvUnit("cm");
        public static EvUnit Px => new EvUnit("px");
    }

    public class EvObject
    {
        private readonly Dict _data;

        public EvObject(Dict data)
        {
            _data = data;
        }

        public int GetId()
        {
            return (int) _data["id"];
        }
    }

    public class EvInsert3DModelOptions
    {
        public EvTesselationQuality TesselationQuality { get; set; }
        public bool UseBrep { get; set; }
        public bool FitToPage { get; set; }

        public EvInsert3DModelOptions(
            EvTesselationQuality tesselationQuality = null,
            bool useBrep = true,
            bool fitToPage = true)
        {
            TesselationQuality = tesselationQuality ?? EvTesselationQuality.ExtraHigh;
            UseBrep = useBrep;
            FitToPage = fitToPage;
        }
    }

    class EvInsertVectorOptions
    {
        public bool BlackBackground { get; set; }
        public bool IgnoreLineWidth { get; set; }
        public bool ExplodeAutocadHatches { get; set; }
        public bool ExplodeAutocadBlocks { get; set; }
        public bool SubstitudeAutocadFontsWithArial { get; set; }
        public bool MergeImportedLayers { get; set; }
        public bool ImportEmptyLayers { get; set; }
        public string LayoutName { get; set; }
        public int LayoutIndex { get; set; }

        public EvInsertVectorOptions(
            bool blackBackground = false,
            bool ignoreLineWidth = false,
            bool explodeAutocadHatches = false,
            bool explodeAutocadBlocks = false,
            bool substitudeAutocadFontsWithArial = false,
            bool mergeImportedLayers = false,
            bool importEmptyLayers = false,
            string layoutName = "Layout 1",
            int layoutIndex = 1)
        {
            BlackBackground = blackBackground;
            IgnoreLineWidth = ignoreLineWidth;
            ExplodeAutocadHatches = explodeAutocadHatches;
            ExplodeAutocadBlocks = explodeAutocadBlocks;
            SubstitudeAutocadFontsWithArial = substitudeAutocadFontsWithArial;
            MergeImportedLayers = mergeImportedLayers;
            ImportEmptyLayers = importEmptyLayers;
            LayoutName = layoutName;
            LayoutIndex = layoutIndex;
        }
    }

    public class EnvOptions
    {
        public EvUnit Units { get; set; }

        public EnvOptions(EvUnit units = null)
        {
            Units = units ?? EvUnit.Px;
        }
    }
}