namespace PacketSnoop.Converters
{
    public class InverseBooleanConverter : BoolToValueConverter<bool>
    {
        public InverseBooleanConverter()
        {
            FalseValue = true;
            TrueValue = false;
        }
    }
}
