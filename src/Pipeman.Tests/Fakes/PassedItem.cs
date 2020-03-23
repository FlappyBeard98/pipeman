namespace Pipeman.Tests.Fakes
{
    public class PassedItem
    {
        public PassedItem(string text, bool @break)
        {
            Text = text;
            Break = @break;
        }

        public string Text { get; }
        public bool Break { get; }
    }
}