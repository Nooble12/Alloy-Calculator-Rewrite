namespace AlloyCalculatorRewrite
{
    public class InputResult
    {
        public bool InputIsValid { set; get; }
        public int InputValue { get; set; }

        public InputResult(bool inputIsValid, int inputValue)
        {
            InputIsValid = inputIsValid;
            InputValue = inputValue;
        }
    }
}
