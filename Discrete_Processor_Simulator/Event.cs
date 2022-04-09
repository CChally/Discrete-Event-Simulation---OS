using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulationStarter
{
    public enum Type { ARRIVAL, DEPARTURE }

    // Class: Event Class
    // Description: The Event class records the arrival and departure of jobs as well as the assignment of processors
    //              dependent on the status of the event. This class will also output messages of the arrival of events
    //              or the departure as well as the interrupts of events.
    // Methods: 1. Event: assigns the job with the time that the event will occur
    //          2. CompareTo: compares two event based on time and defines the priority of events
    class Event : IComparable
    {
        public Job job;
        public double timeOfOccurence;
        public int processor;                // Not assigned until departure
        public Type type = Type.ARRIVAL;     // Defaulted to arrival, every job starts with an arrival
        public double waitingTime = 0;


        // Method: Event 
        // Parameters: exVar, interArrive
        // Returns: void
        public Event(double exVar, double interArrive)
        {
            job = new Job(exVar);                                  // instance of job is created

            Random num = new Random();                             //rng to depict the random time between two arrival events.
            double var = num.NextDouble();

            double extra = -interArrive * (Math.Log(var));

            timeOfOccurence = extra; ;                             // initial time it takes 

        }


        // Method: CompareTO 
        // Parameters: obj
        // Returns: 1. 1    OR
        //          2. 1
        public int CompareTo(object obj)                     // how to compare two events based on time. This defines your priority
        {
            if (obj != null)
            {
                Event e = obj as Event;
                if (e != null)
                {
                    return Convert.ToInt32(job.executionTime - e.job.executionTime);

                }
                else return 1;

            }
            else return 1;
        }
        //------------------------------------------------------------------------------------

        // Following are output messages concerning certain statuses of events -- 

        public void waitingMessage()  // waiting message for a particular event. Interrupts have no waiting messages
        {
            Console.WriteLine($"Job {job.jobID} arrives at {Simulation.printTime()} and is placed in the waiting queue. \n");
            Console.WriteLine("------------------------------------------------------------------------------------------");
        }


        public void assignProcessor(int processorNum)  // turns an arrival event into a departure event
        {
            this.processor = processorNum;
            type = Type.DEPARTURE;
            timeOfOccurence = job.executionTime;
        }


        public void arriveMessage()

        {

            if (job.jobNumber == 1) // if job is regular
            {
                Console.WriteLine($"Job {job.jobID} arrives at {Simulation.printTime()} and begins execution on processor {processor}.\n");
                Console.WriteLine("------------------------------------------------------------------------------------------");
            }
            else // job is interrupt
            {
                Console.WriteLine($"Interrupt 0 arrives at {Simulation.printTime()} and begins execution on processor {processor}\n");
                Console.WriteLine("------------------------------------------------------------------------------------------");
            }
        }



        public void departMessage()
        {

            if (job.jobNumber == 1) //if job is regular
            {
                Console.WriteLine($"Job {job.jobID} completes execution on processor {processor} at {Simulation.printTime()}\n");
                Console.WriteLine("------------------------------------------------------------------------------------------");
            }
            else // job is interrupt
                Console.WriteLine($"Interrupt 0 completes execution on processor {processor} at {Simulation.printTime()}\n");
            Console.WriteLine("------------------------------------------------------------------------------------------");
        }

        public void emptiedMessage(Event e)
        {
            if (job.jobNumber == 1) // job that is emptied is regular
            {
                Console.WriteLine($"Job {job.jobID} completes execution and Job {e.job.jobID} begins execution on processor {e.processor} at {Simulation.printTime()}\n");
                Console.WriteLine("------------------------------------------------------------------------------------------");
            }
            else // job that is emptied is interrupt
            {
                Console.WriteLine($"Interrupt 0 completes execution and Job {e.job.jobID} begins execution on processor {e.processor} at {Simulation.printTime()}\n");
                Console.WriteLine("------------------------------------------------------------------------------------------");
            }

        }

        public void interruptMessage(Event e) // only for interrupt
        {
            Console.WriteLine($"Interrupt 0 arrives and forces the depature event of Job {e.job.jobID} to be placing in the waiting queue.\n");
            Console.WriteLine("------------------------------------------------------------------------------------------");
        }
        public void switchMessage(Event e)
        {
            Console.WriteLine($"Interrupt 0 arrives and begin execution on processor {processor} at {Simulation.printTime()}, trumping out {e.job.jobID} \n");
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine($"Job {e.job.jobID} resumes execution on processor {e.processor} at {Simulation.printTime()}\n");
            Console.WriteLine("------------------------------------------------------------------------------------------");
        }
        public void ignoreMessage(Event e)
        {
            Console.WriteLine($"Interrupt 0 has been ignored on processor {e.processor} at {Simulation.printTime()}\n");
            Console.WriteLine("------------------------------------------------------------------------------------------");
        }

    }

}

