using System;
using System.Collections.Generic;

namespace DriverProgram
{
    using simulationStarter;

    // Class: Driver Class
    // Description: Main class invokes user input and assigned the user input to the variable that corresponds to that value 
    // to be processed. This class includes two main methods that perform their specified duties of output statements and
    // processing user input. 
    // Methods: 1. PrintStart
    //          2. getData
    class Driver
    {
        //-------------------------------------------------------------------------------------------------------------------
        static void Main()
        {
            // variable instatiation: M, T, P
            double interArrival = default(double);          //M
            double executionVariable = default(double);     //T
            int numOfProcessors = default(int);             //P


            PrintStart();                                                               // prints program description
            getData(ref interArrival, ref executionVariable, ref numOfProcessors);      // assigns values using getData class
            Simulation.startSim(interArrival, executionVariable, numOfProcessors);      // starts simulation using Simulation Class
        }
        //----------------------------------------------------------------------------------------------------------------------

        //Method : PrintStart
        //Parameters: null
        //Returns: void
        //Logic : This method simply is made up of writeline statements that describe what the program is, and the values needed from the user and what they mean
        static void PrintStart()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                                              <<Discrete Event Simulator>>" + "\n");  ///display program title
            Console.ResetColor();
            Console.WriteLine("******************************************************************************************************************************");
            Console.WriteLine("This program is a discrete event simulation of an operating system. The program utilizes a priority queue to signify the processing units and how they prioritize certain jobs.");

            Console.WriteLine();
            Console.WriteLine("The program will simulate a 60 minute (3600s) system, that varies depending on user input.");
            Console.WriteLine("The variables in the simulation are as follows:");
            Console.WriteLine();
            Console.WriteLine("'M' --  an exponential distribution MEAN corresponding inter-arrival times between jobs: (Time in seconds) ");
            Console.WriteLine("'T' -- an exponential distribution MEAN corresponding to individual execution times: (Time in seconds)");
            Console.WriteLine("'P' -- a numerical value determining number of processors (size of priority queue) : (Between 1 and 10)");
            Console.Write("\n\n");
        }
        //-------------------------------------------------------------------------------------------------------------------------

        //Method: getData
        //Parameters: M, T, P
        //Returns : Double type specific to the variable (M, T, P) being returned 
        //Logic : This method is 3 embedded methods that loop to get 3 desired variable inputs (M, S, T).
        //        The inputs include the mean interarrival time between jobs, execution time, and number
        //        of processors that was specified by the user.
        static void getData(ref double M, ref double T, ref int P)
        {
            M = getInputM();
            T = getInputT();    // assign each variable to each of three user inputs
            P = getInputP();

            Console.Write("\n" + "Starting simulation based on the variables ...." + "\n");
            Console.WriteLine($" -- Inter-arrival time variable 'M' : {M}");
            Console.WriteLine($" -- Execution time variable 'T'  : {T}");     //display the values that were entered to the user, before sending the parameters off to the simulation class
            Console.WriteLine($" -- Number of Processors 'P' : {P}");
            Console.WriteLine("\n\n");

            Console.ReadKey();                                               //to allow the user to hit any key to complete the simulation. 

            // Embedded Method 1: getInputM
            // Description: validates and requests 'M' variable input
            static double getInputM()
            {
                bool ifValid = false;                               // termination variable
                double M = default(double);                         // assumes default variable 

                Console.ForegroundColor = ConsoleColor.Green;

                // do-while loop using the termination variable 
                do
                {
                    try
                    {
                        Console.Write("Enter the value of the user-defined variable M >>" + " ");
                        M = Convert.ToDouble(Console.ReadLine());
                        if (M > 0)
                            ifValid = true;      // exits loop once the input is valid
                    }
                    catch (FormatException e)    //tests for the input to be a numeric value
                    {
                        Console.WriteLine("Format exception. The input needs to be a number.", e);
                        Console.Write("\n\n\n");

                    }



                } while (!ifValid);
                Console.WriteLine();
                return M;                                   // returns 'M' variable value
            }

            // Embedded Method 2: getInputT
            // Description: validates and requests 'T' variable input
            static double getInputT()
            {
                bool ifValid = false;                              // termination variable
                double T = 0;                                      // default variable and value 
                do
                {
                    try
                    {
                        Console.Write("Enter the value of the user-defined variable T >>" + " ");
                        T = Convert.ToDouble(Console.ReadLine());
                        if (T > 0)
                            ifValid = true;                 // exits loop once the input is valid

                    }
                    catch (FormatException e)               // tests for the input to be a numeric value
                    {
                        Console.WriteLine("Format exception. The input needs to be a number.", e);
                        Console.Write("\n\n\n");

                    }



                } while (!ifValid);
                Console.WriteLine();
                return T;                                   // returns T variable
            }

            // validates and requests the P variable input
            static int getInputP()
            {
                bool ifValid = false;
                int P = default(int);

                do
                {
                    try
                    {


                        Console.Write("Enter the value of the user-defined variable P >> " + " ");
                        P = Convert.ToInt32(Console.ReadLine());
                        if (P >= 1 && P <= 10)
                        {
                            ifValid = true;                                                     // exits loop once the input is valid

                        }
                    }
                    catch (FormatException e)                                                   // exits loop once the input is valid
                    {
                        Console.WriteLine("Format exception. The input needs to be a number.", e);
                        Console.Write("\n\n\n");

                    }




                } while (!ifValid);
                Console.WriteLine();
                return P;                                                // returns 'P' variable value
            }
            //---------------------------------------------------------------------------------------------------------------------------------

        }

    }
}