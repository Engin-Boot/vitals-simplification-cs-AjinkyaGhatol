using System;
using System.Diagnostics;
using System.Collections.Generic;
public delegate void AlertWhenLowOrHighHandler(string alertMessage);
public struct vital
{
    public string vital_id;
    public float minLimit;
    public float maxLimit;
}
public struct vital_array
{
    public string string_id;
    public float val;
}
public class EmailAlert
{
    //Event Handler - Method
    public void Update(string alertMessage)
    {
        Console.WriteLine("Send Email:" + alertMessage);

    }
}
//Subscribers/Observers/EventSink
//here subscribers are not a multiple customers ,it is multiple events we want to trigger when order status change
public class SMSAlert
{
    public void Notify(string alertMessage)
    {
        Console.WriteLine("send SMS:" + alertMessage);
    }

}
class Checker
{
    public static event AlertWhenLowOrHighHandler AlertWhenLowOrHigh;
    static List<vital> vital_list = new List<vital>();
    static int getIndex(string string_id)
    {
        int id = -1;
        for (uint i = 0; i < vital_list.Count; i++)
        {
            if (vital_list[(int)i].vital_id == string_id)
            {
                id = (int)i;
                break;
            }
        }
        return id;
    }
    static bool isOk(int string_index, float val)
    {

        //return(val >= vital_list[id].minLimit && val <= vital_list[id].maxLimit);
        if (val < vital_list[string_index].minLimit)
        {
            string message = "";
            message += "Patients " + vital_list[string_index].vital_id + " is low:" + val.ToString();
            if (AlertWhenLowOrHigh != null)
            {
                AlertWhenLowOrHigh.Invoke(message);
            }
            return false;
        }
        else if (val > vital_list[string_index].maxLimit)
        {
            string message = "";
            message += "Patients " + vital_list[string_index].vital_id + " is high:" + val.ToString();
            if (AlertWhenLowOrHigh != null)
            {
                AlertWhenLowOrHigh.Invoke(message);
            }
            return false;
        }
        return true;
    }

    static void addVital(string string_id, float min, float max)
    {
        vital temp;
        temp.vital_id = string_id;
        temp.maxLimit = max;
        temp.minLimit = min;
        vital_list.Add(temp);
    }
    static int addVital(string string_id)
    {
        float min, max;
        Console.WriteLine("Enter min limit of vital:enterd 100");
        //cin >> min;
        min = 100;
        max = 200;
        Console.WriteLine("Enter max limit of vital:enterd 200");
        //cin >> max;vital temp;
        vital temp;
        temp.vital_id = string_id;
        temp.maxLimit = max;
        temp.minLimit = min;
        vital_list.Add(temp);
        return (vital_list.Count - 1);
    }
    static bool checkVital(string string_id, float val)
    {
        int index = getIndex(string_id);
        if (index != -1)
        {
            return isOk(index, val);
        }
        else
        {
            int index_of_added;
            index_of_added = addVital(string_id);
            return isOk(index_of_added, val);
        }
    }


    static void ExpectTrue(bool expression)
    {
        if (!expression)
        {
            Console.WriteLine("Expected true, but got false");
            Environment.Exit(1);
        }
    }
    static void ExpectFalse(bool expression)
    {
        if (expression)
        {
            Console.WriteLine("Expected false, but got true");
            Environment.Exit(1);
        }
    }
    static bool checkVital(vital_array[] vt_array, int size)
    {
        bool result = true;
        for (int i = 0; i < size; i++)
        {
            if (checkVital(vt_array[i].string_id, vt_array[i].val) == false)
                result = false;
        }
        return result;
    }
    static int Main()
    {
        EmailAlert _emailAlert = new EmailAlert();
        SMSAlert _smsAlert = new SMSAlert();

        AlertWhenLowOrHighHandler _handler_email = new AlertWhenLowOrHighHandler(_emailAlert.Update);
        AlertWhenLowOrHighHandler _handler_sms = new AlertWhenLowOrHighHandler(_smsAlert.Notify);
        addVital("BPM", 70, 150);
        addVital("SPO2", 90, 1000);
        addVital("RESPRATE", 30, 95);

        //true conditions
        ExpectTrue(checkVital("BPM", 100));
        ExpectTrue(checkVital("SPO2", 95));
        ExpectTrue(checkVital("RESPRATE", 60));

        AlertWhenLowOrHigh += _handler_email;
        //false results
        ExpectFalse(checkVital("BPM", 40));
        ExpectFalse(checkVital("SPO2", 89));
        ExpectFalse(checkVital("RESPRATE", 96));
        AlertWhenLowOrHigh -= _handler_email;
        AlertWhenLowOrHigh += _handler_sms;
        ExpectTrue(checkVital("BPM", 80));
        ExpectFalse(checkVital("BPM", 160));

        ExpectTrue(checkVital("SPO2", 95));
        ExpectTrue(checkVital("SPO2", 90));
        ExpectFalse(checkVital("SPO2", 89));


        ExpectTrue(checkVital("RESPRATE", 40));
        ExpectFalse(checkVital("RESPRATE", 100));

        addVital("BP", 100, 200);
        ExpectTrue(checkVital("BP", 150));
        ExpectFalse(checkVital("BP", 80));

        //check vital sugar without adding it first
        //limits for sugar are 100 to 200
        ExpectFalse(checkVital("SUGAR", 300));

        ExpectTrue(checkVital("SUGAR", 200));
        ExpectFalse(checkVital("SUGAR", 50));

        //check multiple vitals at once
        vital_array[] vital_array_to_pass = new vital_array[5];

        //all are within range
        vital_array_to_pass[0].string_id = "BPM";
        vital_array_to_pass[0].val = 140;
        vital_array_to_pass[1].string_id = "SPO2";
        vital_array_to_pass[1].val = 100;
        vital_array_to_pass[2].string_id = "RESPRATE";
        vital_array_to_pass[2].val = 40;
        vital_array_to_pass[3].string_id = "BP";
        vital_array_to_pass[3].val = 150;
        vital_array_to_pass[4].string_id = "SUGAR";
        vital_array_to_pass[4].val = 200;

        ExpectTrue(checkVital(vital_array_to_pass, vital_array_to_pass.Length));

        //BPM and sugar not in limit
        vital_array_to_pass[0].string_id = "BPM";
        vital_array_to_pass[0].val = 69;
        vital_array_to_pass[1].string_id = "SPO2";
        vital_array_to_pass[1].val = 100;
        vital_array_to_pass[2].string_id = "RESPRATE";
        vital_array_to_pass[2].val = 40;
        vital_array_to_pass[3].string_id = "BP";
        vital_array_to_pass[3].val = 150;
        vital_array_to_pass[4].string_id = "SUGAR";
        vital_array_to_pass[4].val = 201;

        ExpectFalse(checkVital(vital_array_to_pass, vital_array_to_pass.Length));


        vital_array[] vital_array_to_pass2 = new vital_array[3];
        //bpm , spo2, resprate are within range
        vital_array_to_pass2[0].string_id = "BPM";
        vital_array_to_pass2[0].val = 100;
        vital_array_to_pass2[1].string_id = "SPO2";
        vital_array_to_pass2[1].val = 95;
        vital_array_to_pass2[2].string_id = "RESPRATE";
        vital_array_to_pass2[2].val = 60;

        ExpectTrue(checkVital(vital_array_to_pass2, vital_array_to_pass2.Length));

        //bpm out of range
        vital_array_to_pass2[0].string_id = "BPM";
        vital_array_to_pass2[0].val = 40;
        vital_array_to_pass2[1].string_id = "SPO2";
        vital_array_to_pass2[1].val = 91;
        vital_array_to_pass2[2].string_id = "RESPRATE";
        vital_array_to_pass2[2].val = 92;

        ExpectFalse(checkVital(vital_array_to_pass2, vital_array_to_pass2.Length));
        Console.WriteLine("All ok");
        return 0;
    }
}