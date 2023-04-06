using System;
using System.Collections.Generic;

namespace cv9_home
{
    internal class Calculator
    {
        private enum Condition
        {
            FirstNum,
            Operator,
            SecondNum,
            Result
        };

        private List<string> basicOperatorsList = new List<string>() { "+", "-", "*", "/" };
        private List<string> complexOperatorsList = new List<string>() { "back", "CE", "C", "+/-" };

        private double storedNumberM = 0;
        private double tempNumber = 0;
        private string operatorCalc = "";
        private Condition _condition = Condition.FirstNum;

        public string display = "0";
        public string memory = "";

        public void ButtonPressed(string buttonContent)
        {
            if (complexOperatorsList.Contains(buttonContent)) // Check group 1 - Complex Operators.
            {
                complexOperator(buttonContent);
                return;
            }
            else if (basicOperatorsList.Contains(buttonContent)) // Check group 2 - Basic Operators.
            {
                operatorCalc = buttonContent;
                _condition = Condition.Operator;
                if (tempNumber != 0)
                {
                    countNums();
                    tempNumber = Double.Parse(display);
                }
                return;
            }
            else if (buttonContent.Substring(0,1) == "M") // Check group 3 - M Operators.
            {
                mAryOperator(buttonContent);
                return;
            }
            switch (_condition) // Only numbers, "," and "=" left
            {
                case Condition.FirstNum:
                    parseNums(buttonContent);
                    break;
                case Condition.Operator:
                    _condition = parseNums(buttonContent) ? Condition.SecondNum : _condition;
                    break;
                case Condition.SecondNum:
                    if (parseNums(buttonContent))
                        return;
                    if (buttonContent == "=")
                        countNums();
                    break;
                case Condition.Result:
                    if(parseNums(buttonContent))
                        _condition = Condition.FirstNum;
                    break;
            }

        }

        private bool parseNums(string buttonContent)
        {
            if (Double.TryParse(buttonContent, out double result) || buttonContent == ",")
            {
                if (_condition == Condition.Operator)
                {
                    _condition = Condition.SecondNum;
                    tempNumber = Double.Parse(display);
                    display = "0";
                }
                else if (_condition == Condition.Result)
                {
                    _condition = Condition.FirstNum;
                    tempNumber = Double.Parse(display);
                    display = "0";
                }

                display = display == "0" ? buttonContent : display + buttonContent;

                return true;
            }
            return false;
        }

        private void mAryOperator(string buttonContent)
        {
            memory = "M";
            switch (buttonContent)
            {
                case "MS":
                    storedNumberM = double.Parse(display);
                    break;
                case "MR":
                    if (_condition == Condition.Operator) {
                        tempNumber = double.Parse(display);
                        _condition = Condition.SecondNum;
                    }
                    display = storedNumberM.ToString();
                    break;
                case "MC":
                    storedNumberM = 0;
                    memory = "";
                    break;
                case "M+":
                    storedNumberM += double.Parse(display);
                    break;
                case "M-":
                    storedNumberM -= double.Parse(display);
                    break;
            }
        }

        private void complexOperator(string buttonContent)
        {
            if (buttonContent == "CE")
                display = "0";
            else if (buttonContent == "back")
                display = display.Length > 1 ? display.Substring(0, display.Length - 1) : "0";
            else if (buttonContent == "+/-")
                display = display.Substring(0,1) == "-" ? display.Substring(1, display.Length - 1) : "-" + display;
            else if (buttonContent == "C")
            {
                _condition = Condition.FirstNum;
                tempNumber = 0;
                display = "0";
                operatorCalc = "";
            }
        }

        private void countNums()
        {
            if(_condition != Condition.FirstNum)
            {
                _condition = Condition.Result;
            }
            double tempDisp = Double.Parse(display);
            switch(operatorCalc)
            {
                case "+":
                    display = (tempNumber + tempDisp).ToString();
                    break;
                case "-":
                    display = (tempNumber - tempDisp).ToString();
                    break;
                case "*":
                    display = (tempNumber * tempDisp).ToString();
                    break;
                case "/":
                    display = (tempNumber / tempDisp).ToString();
                    break;
            }
            tempNumber = 0;
        }
    }
}