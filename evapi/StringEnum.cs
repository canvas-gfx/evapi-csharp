namespace evapi
{
    public class StringEnum
    {
        public StringEnum(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(StringEnum sEnum)
        {
            return sEnum.ToString();
        }
    }
}