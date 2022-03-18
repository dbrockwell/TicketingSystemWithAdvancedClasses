﻿using System;
using System.IO;
using NLog.Web;

namespace ticketsV3
{
    class Program
    {
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static async void Main(string[] args)
        {
            string ticketFilePath1 = Directory.GetCurrentDirectory() + "\\tickets1.csv";
            string ticketFilePath2 = Directory.GetCurrentDirectory() + "\\tickets2.csv";
            string ticketFilePath3 = Directory.GetCurrentDirectory() + "\\tickets3.csv";
            logger.Info("Program started");
            TicketFile ticketFile = new TicketFile(ticketFilePath1, ticketFilePath2, ticketFilePath3);
            string choice;
            do {
                Console.WriteLine("1) Read ticket information");
                Console.WriteLine("2) Add ticket infomation");
                Console.WriteLine("Enter any other key to exit");
                choice = Console.ReadLine();

                if (choice == "1") {
                    Console.WriteLine("Bug Tickets\n");
                    foreach(Ticket ticket in ticketFile.Bugs)
                    {
                        Console.WriteLine(ticket.entry());
                    }
                    Console.WriteLine("Enhancement Tickets\n");
                    foreach(Ticket ticket in ticketFile.Enhancements)
                    {
                        Console.WriteLine(ticket.entry());
                    }
                    Console.WriteLine("Task Tickets\n");
                    foreach(Ticket ticket in ticketFile.Tasks)
                    {
                        Console.WriteLine(ticket.entry());
                    }
                }
                
                if (choice == "2") {
                    Console.WriteLine("1) Enter a bug ticket");
                    Console.WriteLine("2) Enter a enhancement ticket");
                    Console.WriteLine("3) Enter a task ticket");
                    Console.WriteLine("Enter anything else to exit");
                    string ticketChoice = Console.ReadLine();
                    if (ticketChoice == "1") {
                        Bug bug = new Bug();
                        StandardInfo(bug);
                        bug.severity = NullCheck("Enter Ticket Severity", "severity");
                        ticketFile.AddBugTicket(bug);
                    }
                    else if (ticketChoice == "2") {
                        Enhancement enhancement = new Enhancement();
                        StandardInfo(enhancement);
                        enhancement.software = NullCheck("Enter Ticket Software", "software");
                        bool contineAdd = true;
                        do{
                            try {
                                enhancement.cost = Double.Parse(NullCheck("Enter Ticket Cost", "cost"));
                            } catch (Exception){
                                logger.Error("Not a correct number");
                                contineAdd = false;
                            }
                        } while (contineAdd == false);
                        enhancement.reason = NullCheck("Enter Ticket Reason", "reason");
                        do{
                            try {
                                enhancement.estimate = Double.Parse(NullCheck("Enter Ticket Estimate", "estimate"));
                            } catch (Exception){
                                logger.Error("Not a correct number");
                                contineAdd = false;
                            }
                        } while (contineAdd == false);
                        ticketFile.AddEnhancementTicket(enhancement);
                    }
                    else if (ticketChoice == "3") {
                        Task task = new Task();
                        StandardInfo(task);
                        task.projectName = NullCheck("Enter Ticket Project Name", "project name");
                        string dateEntry = NullCheck("Enter Ticket Due Date (Month/Day/Year", "due date");
                        string[] date = dateEntry.Split("/");
                        bool dateCheck = true;
                        do {
                            try {
                                int month = Int32.Parse(date[0]);
                                int day = Int32.Parse(date[1]);
                                int year = Int32.Parse(date[2]);
                            } catch (Exception) {
                                logger.Error("Incorrect date entered");
                                dateCheck = false;
                            }
                        } while(dateCheck == false);
                        ticketFile.AddTaskTicket(task);
                    }
                }
            } while (choice == "1" || choice == "2");

            logger.Info("Program Ended");
        }

        public static void StandardInfo(Ticket ticket) {
            ticket.summary = NullCheck("Enter Ticket Summary", "summary");
            ticket.status = NullCheck("Enter Ticket Status", "status");
            ticket.priority = NullCheck("Enter Ticket Priority", "priority");
            ticket.submitter = NullCheck("Enter the Ticket Submitter", "submitter");
            ticket.assigned = NullCheck("Enter Person Assigned", "assigned");
            ticket.peopleWatching.Add(NullCheck("Enter Person Watching", "person watching"));
            string anotherWatcher;
            do {
                Console.WriteLine("Enter Another Person Watching Or Just Press 'ENTER' To Continue");
                anotherWatcher = Console.ReadLine();
                if (anotherWatcher != "") {
                    ticket.peopleWatching.Add(anotherWatcher);
                }
            } while (anotherWatcher != "");
        }

        public static string NullCheck(string question, string errorName) {
            bool continueLoop = true;
            string entry;
            do {
                Console.WriteLine(question);
                entry = Console.ReadLine();
                if (entry == "") {
                    logger.Error("No input for {0} was entered", errorName);
                    continueLoop = true;
                }
                else {
                    continueLoop = false;
                }
            } while (continueLoop == true);

            return entry;
        }
    }
}
