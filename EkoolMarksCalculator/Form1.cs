using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace EkoolMarksCounter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();


        private void button1_Click(object sender, EventArgs e) //Button click event
        {
            openFileDialog1.ShowDialog(); //Opens file dialogue
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e) //File opened event
        {
            dataGridView1.Rows.Clear();

            doc.Load(openFileDialog1.FileName); //Loads HTML document from file


            var rows = doc.DocumentNode.SelectNodes("//table[@class='fulltable gradesheet velvet-table gray-headers kov']/tbody/tr"); //Selects rows from table
            Dictionary<string, List<int>> grades = new Dictionary<string, List<int>>(); //Creates dictionary with scheme <subject(key), List<grades>(value)>



            for (int i = 0; i < rows.Count; i++) //I'm stupid and too lazy to fix that
            {
                string name = rows[i].SelectNodes("//div[@class='name']")[i].InnerText.Trim(); //Gets name of a subject


                var gradeElements = rows[i].ChildNodes[7].ChildNodes;
                

                grades.Add(name, new List<int>()); //Adds subject to dictionary

                 for (int x = 0; x < gradeElements.Count; x++)//adds every mark to list
                 {

                     if (gradeElements[x].ChildNodes[1].InnerText.Trim().Trim('*') == "1" || gradeElements[x].ChildNodes[1].InnerText.Trim().Trim('*') == "2" || gradeElements[x].ChildNodes[1].InnerText.Trim().Trim('*') == "3" || gradeElements[x].ChildNodes[1].InnerText.Trim().Trim('*') == "4" || gradeElements[x].ChildNodes[1].InnerText.Trim().Trim('*') == "5")//Checks for a mark
                     {
                         int grade = Int32.Parse(gradeElements[x].ChildNodes[1].InnerText.Trim().Trim('*')); //creates a Int variable of it
                         grades[name].Add(grade); //adds it to the list
                     }

                 }
                 if (grades[name].Count == 0) //If there is no marks, delete it
                 {
                     grades.Remove(name);
                 }
             }

             foreach (var name in grades.Keys)
             {
                 dataGridView1.Rows.Add(name, grades[name].Average(), Math.Round(grades[name].Average()), countMarksUntilFive(grades[name])); //Adds data to dataGridView (name of a subject, average mark, rounded average, five marks you need to get mark 5 in total)
             }

            }

             string countMarksUntilFive(List<int> grades) //method to count a count of five marks to get mark five in total
             {
                 int marksUntilFive = 0;
                 string marksUntilFiveResultString = "";

                 double average = grades.Average(); //gets average mark

                 if (Math.Round(average) == 5) //If you already have 5 it returns 0
                 {
                     return "0";
                 }
                 else
                 {
                     while (Math.Round(average) != 5) //does code while mark isn't 5
                     {
                         grades.Add(5); //add 5 to grades list
                         marksUntilFive++; //adds one to marks until five count
                         average = grades.Average(); //counts average again
                     }
                     marksUntilFiveResultString = marksUntilFive.ToString(); //makes count string
                 }
                 return marksUntilFiveResultString; //returns result
             }




        }
    }

