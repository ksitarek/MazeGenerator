using System.Windows.Controls;

namespace MazeGenerator
{
    /// <summary>
    /// Interaction logic for IntUpDown.xaml
    /// </summary>
    public partial class IntUpDown : UserControl
    {
        private int _numValue;

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                _textBox.Text = _numValue.ToString();
            }
        }

        public int MinValue { get; set; } = int.MinValue;
        public int MaxValue { get; set; } = int.MaxValue;

        public IntUpDown()
        {
            InitializeComponent();
            _textBox.Text = NumValue.ToString();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(_textBox.Text, out _numValue))
                _textBox.Text = _numValue.ToString();

            if (NumValue > MaxValue)
                NumValue = MaxValue;

            if (NumValue < MinValue)
                NumValue = MinValue;
        }

        private void UpBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NumValue < MaxValue)
                NumValue++;
        }

        private void DownBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NumValue > MinValue)
                NumValue--;
        }
    }
}