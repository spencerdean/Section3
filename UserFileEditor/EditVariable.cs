using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserFileEditor
{
    class EditVariable
    {
        public string Edit(string fieldValue)
        {
            string newValue = fieldValue; // making a copy of fieldValue that we can manipulate (for data integrity)
            var xPos = 0;
            var yPos = Console.WindowTop + Console.WindowHeight - 1;
            Console.SetCursorPosition(xPos, yPos);
            ConsoleKeyInfo keyinfo;
            int insertCounter = 0;
            bool terminateLoop = false;
            do
            {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.Insert)
                {
                    insertCounter++;
                    if (insertCounter % 2 == 1)
                    {
                        Console.CursorSize = 100;
                        //change input method here
                        int loopCounter = 1;
                        for (int i = 0; i < loopCounter; i++)
                        {
                            ConsoleKeyInfo otherKey = Console.ReadKey(true);
                            var readVal = ' ';
                            readVal = otherKey.KeyChar;
                            var arrowDetect = (char)otherKey.Key;

                            if (char.IsLetter(arrowDetect) || char.IsDigit(arrowDetect))
                            {
                                if (newValue.Length > 1)
                                {
                                    if (newValue.Length <= xPos + 1)
                                    {
                                        newValue = string.Concat(newValue, readVal.ToString());
                                    }
                                    else
                                    {
                                        newValue = newValue.Remove(xPos, 1);
                                        newValue = newValue.Insert(xPos, readVal.ToString());
                                    }
                                }
                                else
                                {
                                    newValue = newValue.Insert(xPos, readVal.ToString());
                                }
                                Console.Write(readVal);
                                xPos++;
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else if (arrowDetect == (char)Keys.Left)
                            {
                                if (xPos > 0)
                                {
                                    xPos--;
                                    Console.SetCursorPosition(xPos, yPos);
                                    loopCounter++;
                                }
                            }
                            else if (arrowDetect == (char)Keys.Right)
                            {
                                if (xPos < newValue.Length)
                                {
                                    xPos++;
                                    Console.SetCursorPosition(xPos, yPos);
                                    loopCounter++;
                                    continue;
                                }
                            }
                            else if (arrowDetect == (char)Keys.Escape)
                            {
                                // escape method here
                                newValue = string.Empty;
                                xPos = 0;
                                Console.SetCursorPosition(xPos, yPos);
                                Console.Write("                      ");
                                xPos = 0;
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else if (arrowDetect == (char)Keys.Enter)
                            {
                                // enter method here
                                if (fieldValue.Contains("/"))
                                {
                                    // Date of Birth - need to validte
                                    DateTime newDOB;
                                    if (DateTime.TryParse(fieldValue, out newDOB))
                                    {
                                        fieldValue = newValue;
                                        Console.WriteLine("User has been successfully updated");
                                    }
                                    else
                                    {
                                        Console.Error.WriteLine("Error: Invalid Date of Birth entered ");
                                    }
                                }
                                else
                                {
                                    // not a Date of Birth - no need to validate
                                    fieldValue = newValue;
                                    //Console.WriteLine();
                                    //Console.WriteLine("User has been successfully updated");
                                }
                                terminateLoop = true; // exit loop
                                loopCounter++;
                                break;
                            }
                            else if (arrowDetect == (char)Keys.Home)
                            {
                                xPos = 0;
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else if (arrowDetect == (char)Keys.Delete)
                            {
                                // delete method here
                                int bspaceXpos = 0;
                                for (int j = 0; j < newValue.Length; j++)
                                {
                                    Console.SetCursorPosition(bspaceXpos, yPos);
                                    Console.Write(" ");
                                    bspaceXpos++;
                                }
                                Console.SetCursorPosition(0, yPos);
                                if (newValue.Length > 1 && newValue.Length != (xPos + 1))
                                {
                                    newValue = newValue.Remove(xPos + 1, 1);
                                }
                                else if (newValue.Length > 1 && newValue.Length == (xPos + 1))
                                {
                                    newValue = newValue;
                                }
                                else if (newValue.Length == 1)
                                {
                                    newValue = string.Empty;
                                }
                                else
                                {
                                    continue;
                                }
                                Console.Write(newValue);
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else if (arrowDetect == (char)Keys.Back)
                            {
                                // backspace method here
                                int bspaceXpos = 0;
                                for (int j = 0; j < newValue.Length; j++)
                                {
                                    Console.SetCursorPosition(bspaceXpos, yPos);
                                    Console.Write(" ");
                                    bspaceXpos++;
                                }
                                Console.SetCursorPosition(0, yPos);
                                if (newValue.Length > 0 && xPos > 0)
                                {
                                    newValue = newValue.Remove(xPos - 1, 1);
                                    xPos--;
                                }
                                else if (newValue.Length == 0)
                                {
                                    newValue = string.Empty;
                                }
                                else if (newValue.Length > 0 && xPos <= 1)
                                {
                                    continue;
                                }
                                else
                                {
                                    continue;
                                }
                                Console.Write(newValue);
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;

                            }
                            else if (arrowDetect == (char)Keys.End)
                            {
                                xPos = newValue.Length;
                                Console.SetCursorPosition(xPos, yPos);
                                loopCounter++;
                            }
                            else
                            {
                                loopCounter++;
                            }
                        }
                    }
                    else
                    {
                        Console.CursorSize = 1;
                    }

                    if (keyinfo.Key == ConsoleKey.LeftArrow && xPos > 0)
                    {
                        xPos--;
                        Console.SetCursorPosition(xPos, yPos);
                    }
                    else if (keyinfo.Key == ConsoleKey.RightArrow)
                    {
                        if (xPos < newValue.Length)
                        {
                            xPos++;
                            Console.SetCursorPosition(xPos, yPos);
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (keyinfo.Key == ConsoleKey.LeftArrow && xPos > 0)
                    {
                        xPos--;
                        Console.SetCursorPosition(xPos, yPos);
                    }
                    else if (keyinfo.Key == ConsoleKey.RightArrow)
                    {
                        if (xPos < newValue.Length)
                        {
                            xPos++;
                            Console.SetCursorPosition(xPos, yPos);
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            while (terminateLoop == false);
            Console.WriteLine();
            Console.WriteLine("End of program. The new value you entered is " + fieldValue);
            System.Threading.Thread.Sleep(2000);
            return fieldValue;
        }
    }
}
