using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulationStarter
{
    // Class: Simulation Class
    // Description: This class is the main class that runs and interprets the parameters previously provided by the
    //              user in the Main class. This class includes two methods and 5 static bool methods that act as 
    //              initializing methods for specfied values.
    // Methods: 1. Simulation Constructor: this constructor initializes 4 data members that are
    //             involved in the queue creation and the status of processors 
    //          2. startSim: this method is the main method that begins the simulation and diffrentiates between
    //                       arrivals and departures as well as interupts. 
    // Bool Methods: 1. shutDown()
    //               2. bothEmpty()
    //               3. waitingEmpty()
    //               4. priorityEmpty()
    //               5. priorityFull()
    class Simulation
    {
        //data members
        public static double currRunTime = 0;       // Current run time of simulation. Starts at 0:00 and increases in seconds to 3600 
        public static double totalWaitTime = 0;     // Total wait time, starts at 0:00 and increases by how much time spent in the waiting queue. 

        public static Queue<Event> waiting;             // waiting queue
        public static PriorityQueue<Event> priority;    // priority queue
        public static bool[] ifActive;                  // boolean array with P elements to correlating with active/non-active processors
        public static int priorityMax;
        //--------------------------------------------------------------------------------------------------------------

        // Constructor 1: Simulation Constructor
        // Parameters: 1. waiting
        //             2. priority
        //             3. ifActive
        //             4. priorityMax
        public Simulation(int numOfProcessors)   //Simulation Constructor
        {
            waiting = new Queue<Event>();                                // waiting queue of events; arbitrary size in length
            priority = new PriorityQueue<Event>(2 * numOfProcessors);    // constant for heavy load, medium load, and light load. This is the priority queue of events with size 2p
            ifActive = new bool[numOfProcessors + 1];                    // creates boolean array with same indexing system as the priority queue array. Can be indexed through 0 to 10.
            priorityMax = 2 * numOfProcessors;
        }
        //---------------------------------------------------------------------------------------------------------------

        // Method: startSim
        // Description: this method uses the value of M, P and T to begin the simulation with the first event and
        //                       so on with arrivals and departures of events, either linking an event up with a processor,
        //                       or putting it in a waiting queue. This method also deals with interupts. 
        public static void startSim(double M, double T, int P)
        {

            Simulation sim = new Simulation(P);             // Create simulation instance
            Event newEvent = new Event(T, M);               // Creates first event, places it in the priority queue as a starting point
                                                            // This event will be removed which will trigger the other process instances 

            priority.Add(newEvent);                         // insert starting event into array

            while (!shutDown())                             // continue simulation while the 60m timer has not ended
            {
                bool found = false;                         // bool variable for finding available processors 
                bool complete = false;
                int index = default(int);                    // variable for processors found
                int j = default(int);
                int ind = default(int);

                if (priority.Front().type == Type.ARRIVAL) //if the event is an arrival     ARRIVAL
                {
                    //------------------------------------------------------------------------------------
                    if (priority.Front().job.jobNumber == 1) // if job is regular       ARRIVAL REGULAR
                    {
                        for (int i = ifActive.Length - 1; i >= 1; i--) // Iterate through boolean array, and store the index of the first available processor.
                                                                       // Loop starts from p = 10 and keeps updating the index value as it goes to p = 1. (Processors fill from left to right)
                        {
                            if (ifActive[i] == false)
                            {
                                index = i;                             // updates to the last found availible processor 

                            }
                            if (index > 0)                             // if it doesnt still have the defaulted value.
                            {
                                found = true;                          // set found flag to true, meaning an available processor has been found
                            }

                            //do nothing
                        }



                        // If 'found' - departure event is created and placed in the priority queue
                        if (found)
                        {
                            //display arrival message
                            ifActive[index] = true;                                     //assign boolean array at open index to be active

                            currRunTime = currRunTime + priority.Front().timeOfOccurence;
                            priority.Front().assignProcessor(index);                    // Make the event into a departure type and assign the departure event a processor
                                                                                        // Give the event a departure time and an assigned processor
                            priority.Front().arriveMessage();
                            Event newArrival = new Event(T, M);



                            priority.Add(newArrival);
                        }

                        // No processor available - arrival is placed in waiting queue
                        else
                        {
                            currRunTime = currRunTime + priority.Front().timeOfOccurence;

                            priority.Front().waitingMessage();              // waiting queue message displayed
                            waiting.Enqueue(priority.Front());              //queue the arrival event
                            priority.Remove();                              //remove from priority queue

                            Event newArrival = new Event(T, M);

                            priority.Add(newArrival);
                        }



                    }
                    //-----------------------------------------------------------------------------------------
                    else //job is interrupt                                            ARRIVAL INTERRUPT

                    {
                        //choose a random processor to interrupt
                        //if the processor has an active interrupt, then the interrupt is ignored
                        //The search will then begin again for an active processor with a regular job

                        Random random = new Random(); // generate random selected index 
                        int number = (int)random.Next(1, priority.Count);    //1 - 10 (processor num)




                        if (ifActive[number] == false) //if there is available space, then the interrupt processes on the random processor
                        {
                            ifActive[number] = true;                                      // set index to active
                            currRunTime = currRunTime + priority.Front().timeOfOccurence; // increment the time for this arrival
                            priority.Front().assignProcessor(number);                     // make the arrival into a departure event -- assign it a processor
                            priority.Front().arriveMessage();                             // arrival message

                            Event newArrival = new Event(T, M);                         //create new arrival

                            priority.Add(newArrival);                                   //place in waiting queue

                        }

                        else //there is something to interrupt : a regular job or an interrupt
                        {

                            //determine if the job that is being interrupted is a regular job or an interrupt
                            for (int i = 1; i <= priority.Count; i++)
                            {

                                if (priority.Peek(i).processor == number) //finding the corresponding departure event assigned to the random generated processor
                                {
                                    ind = i;

                                    if (priority.Peek(ind).job.jobNumber == 0) // the job at the processor is an interrupt
                                    {
                                        currRunTime = currRunTime + priority.Front().timeOfOccurence;
                                        priority.Front().ignoreMessage(priority.Peek(ind));
                                        priority.Remove();

                                        Event newArrival = new Event(T, M); //create new event

                                        priority.Add(newArrival);                                                           // ignore message
                                                                                                                            // try new processor
                                    }
                                    else //the job at the processor is regular
                                    {

                                        for (j = ifActive.Length - 1; j >= 1; j--) //iterate through boolean array, and store the index of the first available processor. Loop starts from the end (p=10) and keeps updating the index value as it goes to p=1. (Processors fill from left to right)
                                        {
                                            if (ifActive[j] == false)
                                            {
                                                index = j; // updates to the last found avaialble processor which will be          || BOOLEAN ARRAY SEARCH ||

                                            }
                                            if (index > 0) // if it doesnt still have the defaulted value.
                                            {
                                                found = true; // set found flag to true, meaning an available processor has been found
                                            }


                                        }
                                        if (found) //open processor found
                                        {
                                            ifActive[index] = true;                                         // set the found open index to true -- it is now active
                                            priority.Peek(ind).processor = index;                           // reassign the processor to the open one, and resumes
                                            currRunTime = currRunTime + priority.Front().timeOfOccurence;
                                            priority.Front().assignProcessor(number);                       // create depature event for interrupt-- assign it a processor
                                            priority.Front().switchMessage(priority.Peek(ind));             // display arrival, and reassign
                                            Event newArrival = new Event(T, M);                             // create new event

                                            priority.Add(newArrival);                                       // add new arrival

                                        }
                                        else    //no open processor found
                                        {
                                            currRunTime = currRunTime + priority.Front().timeOfOccurence;   // increment time
                                            priority.Front().assignProcessor(number);                       // assign processor to interrupt
                                            priority.Front().interruptMessage(priority.Peek(ind));          // display arrival
                                            waiting.Enqueue(priority.Peek(ind));                            // queue item in waiting 
                                            priority.RemoveAt(ind);                                         // remove departure event from priority queue
                                            Event newArrival = new Event(T, M);                             // create arrival

                                            priority.Add(newArrival);                                       //trigger new arrival 


                                        }

                                        // if there is, the departure event in the priority queue is updated to the newly assigned processor
                                        // else the job is removed from the priority queue using removeat method
                                        // It is placed it the waiting queue and its execution time is reduced by the amount of time that has already been devoted to its execution
                                        // can resume execution on any processor
                                    }

                                }
                            }
                        }

                    }








                    //-----------------------------------------------------------------------------------------
                }
                else //event is a departure , same for interrupts and regular jobs                                              DEPARTURE
                {
                    ifActive[priority.Front().processor] = false;               // set the boolean array for the index to be false, meaning a processor has been opened up

                    if (!waitingEmpty())                                        // if the waiting queue has items
                    {
                        currRunTime = currRunTime + priority.Front().job.executionTime;
                        totalWaitTime = totalWaitTime + priority.Front().timeOfOccurence + waiting.Peek().timeOfOccurence;      //increment the total wait time the job in the waiting queue had to experience  
                        waiting.Peek().assignProcessor(priority.Front().processor);                                             //make a departure event for the incoming, assign it a processor
                        ifActive[waiting.Peek().processor] = true;                                                              //set the boolean array for the new event element to be true; (PROCESSOR IS ACTIVE)
                        priority.Front().emptiedMessage(waiting.Peek());                                                        //display pre emptied message                                                 
                        priority.Remove();                                                                                      //remove the departure event from the priority queue
                        priority.Add(waiting.Peek());                                                                           //add new departure event to priority queue
                        waiting.Dequeue();                                                                                      //dequeue the event from the waiting queue


                    }
                    else // waiting queue is empty, processor is available for next job to arrive
                    {
                        currRunTime = currRunTime + priority.Front().job.executionTime;
                        priority.Front().departMessage();                                   // depart message is displayed
                                                                                            //increment the time it took for the departure
                        priority.Remove();                                                  // remove departure event from priority queue -- generates new Front()

                    }

                    //------------------------------------------------------------------------------------------------    
                }
            }
            if (totalWaitTime > 0)
            {
                double result = (double)totalWaitTime / Job.numofRegJobs;                                                                                                      //AVERAGE WAIT TIME CALCULATION
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The average wait time for an event is {result} seconds.");          //print average wait time
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There were no events being enqueued to the waiting queue. 0s!");     //print average wait time
                Console.ResetColor();
            }




            while (!bothEmpty()) //once 60m timer is up, then stop creating events and process remaining jobs currently in queues. 
            {
                //process the remaining elements until both the priority queue and the waiting queue are empty.
            }

        }

        public static bool shutDown()
        {
            return currRunTime >= 3600; //termination time to stop the simulation
        }
        public static bool bothEmpty()
        {
            return (waiting.Count == 0 && priority.Count == 0);
        }
        public static bool waitingEmpty()
        {
            return waiting.Count == 0;
        }
        public static bool priorityEmpty()
        {
            return priority.Count == 0;
        }
        public static bool priorityFull()
        {
            return priority.Count == priorityMax;

        }

        // String method
        // Description: outputs the time in strings
        public static string printTime()
        {
            int seconds = (int)currRunTime % 60;            // remainder of runtime/ 60 to get seconds into minutes
            int mins = (int)currRunTime / 60;               // NOTICE: : integer downcast
            int hours = 0;
            string minsString = mins.ToString();
            string secondsString = seconds.ToString();

            if (seconds < 10)
            {
                secondsString = $"0{seconds}";
            }

            if (mins < 10)
            {
                minsString = $"0{mins}";
            }
            if (mins >= 60)
            {
                hours = 1;
                minsString = $"00";
                secondsString = $"00";



            }
            return $"0{hours}:{minsString}:{secondsString}";
        }
    }



}