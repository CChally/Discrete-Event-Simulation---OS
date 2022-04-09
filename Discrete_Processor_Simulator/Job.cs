using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace simulationStarter
{



    public class Job
    {
        public int jobID = 0;               // unique job number that increments only for regular jobs, and starting a 0
        public int jobNumber = 1;           // jobNumber for exceptions. --> Initialize to 1 for boolean statements in the priority queue 
                                            // 1 --> regular job, 0 --> interrupt 
                                            // Unambiguous indentifier for distinguishing job types.
                                            // If the job is an interrupt, then the non-static jobNumber varaible will initialize

        public static int counter = 0;

        public static int numofRegJobs = 0;    //static counter for number of regular jobs created throughout entire simulation



        public double executionTime;  //remaining execution time of the job in seconds. 

        //Method: Job()
        //Parameters: double executionVariable given by user
        //Returns: void
        //Determines: remaining execution time for a job
        //            if it is a regular or interrupt 
        public Job(double executionVariable)
        {
            //REMAINING EXECUTION TIME FOR A JOB


            //create random number object, generate a number between 0-1 to calculate execution time with some deviation from MEAN
            Random num = new Random();
            double u = num.NextDouble();

            //get execution time by using the exponential function with a MEAN of 'executionVariable' which is 'T'
            //function parameter given by user 
            executionTime = -executionVariable * (Math.Log(u));





            //REGULAR JOB VS INTERRUPT RNG

            double whatType = num.NextDouble(); // next random number to generate a number from 0 to 1. 

            //if the random number < .90, the job type is REGULAR
            //else job type is INTERRUPT
            if (whatType < .90)
            {
                counter = Interlocked.Increment(ref counter);
                jobID = counter + jobID;

                numofRegJobs = jobID;

            }
            else
            {
                jobNumber = 0; //interrupt, identified by jobNum = 0
            }



        }



    }
}