using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Ado.net.Constants
{
    public static class Messages
    {
        public static void InvalidInputMessage(string title) => Console.WriteLine($"{title} is invalid, please try again");
       
        public static void InputMessage(string title) => Console.WriteLine($"input {title}:");
     

        public static void SuccesMessage(string title, string value) => Console.WriteLine($"{title}  {value} succesfully added");
     

        public static void SuccesUpdatedMessage(string title, string value) => Console.WriteLine($"{title}  {value} succesfully updated");



        public static void SuccesDeleteMessage(string title, string value) => Console.WriteLine($"{title}  {value} succesfully deleted");
        
        public static void ErrorOccuredMessage() => Console.WriteLine("Error occured, Please try again");
        

        public static void AlreadyExistMessage(string title, string value) => Console.WriteLine($"{title}  {value} is already exist");


        public static void PrintMessage(string title, string value) => Console.WriteLine($"{title} - {value}");


        public static void NotFoundMessage(string title, string value) => Console.WriteLine($"{title} {value} not found");

        public static void PrintWantToChangeMessage(string title) => Console.WriteLine($"Do you want to change {title}? Y or N");



    }

}
