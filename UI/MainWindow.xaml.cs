using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;




namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateProcessList();

            //this.ProcessListView.Items.Add(new Proc { Command = "", Name = "David" });
        }

        private void UpdateProcessList(int index = 0)
        {
            this.ProcessListView.SelectedItem = null;
            this.ProcessListView.Items.Clear();

            var procList = Properties.Settings.Default.Processes.Cast<string>().ToArray();
            var nameList = Properties.Settings.Default.Names.Cast<string>().ToArray();

            var col = new List<Proc>();

            procList.ToList().ForEach(x => {
                var ix = procList.ToList().IndexOf(x);

                var proc = x;
                var name = nameList.ToList().ElementAt(ix);

                
                col.Add(new Proc { Command = proc, Name = name });

                //this.ProcessListView.Items.Add(new Proc { Command = proc, Name = name });
            });

            List<Proc> SortedList = col.OrderBy(o => o.Name).ToList();

            SortedList.ForEach(x => {
                this.ProcessListView.Items.Add(new Proc { Command = x.Command, Name = x.Name });
            });


            this.ProcessListView.SelectedIndex = index;
        }


        private void SetBtn_Click(object sender, RoutedEventArgs e)
        {
            var command = this.ProcessCommandTextBox.Text;
            var name = this.NicknameTextBox.Text;

            Properties.Settings.Default.Processes.Add(command);
            Properties.Settings.Default.Names.Add(name);

            this.ProcessListView.Items.Add(new Proc { Command = command, Name = name });
            Properties.Settings.Default.Save();
            Console.WriteLine("Added");
        }


        private void ProcessListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.ProcessListView.SelectedItem != null) {
                Console.WriteLine("Selection Changed");
                var selection = (Proc)this.ProcessListView.SelectedItem;
                this.SelectedItemNickNameTextBox.Text = selection.Name.ToString();
                this.SelectedItemCommandTextBox.Text = selection.Command.ToString();

                //To minimize the period of time where there's no window active
                //Lets Maximize the selected window first, then minimize the rest

                ShowWindow.maximize(selection.WindowHandle);

                var list = this.ProcessListView.Items;
                foreach (Proc item in list) {
                    if (item.WindowHandle != System.IntPtr.Zero) {
                        if(selection.WindowHandle == item.WindowHandle) {
                            //Console.WriteLine("Found Handle");
                            //ShowWindow.maximize(item.WindowHandle);
                        } else {
                            ShowWindow.minimize(item.WindowHandle);
                        }
                    }
                }

            }


        }

        private void StartProcessButton_Click(object sender, RoutedEventArgs e)
        {
            var selection = (Proc)this.ProcessListView.SelectedItem;
            if( selection.WindowHandle == System.IntPtr.Zero ) {
                var split = selection.Command.Split();

                var parsePath = ParseFilePath(split[0]);
                var arguments = selection.Command.Remove(0, selection.Command.IndexOf(' ') + 1);

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = parsePath[1];
                startInfo.Arguments = arguments;
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = true;
                
                startInfo.WorkingDirectory = parsePath[0];

                var process = Process.Start(startInfo);
                process.WaitForInputIdle();
                var handle = process.MainWindowHandle;

                process.EnableRaisingEvents = true;
                process.Exited += (origin, eventArgs) => {
                    selection.WindowHandle = IntPtr.Zero ;
                };

                selection.WindowHandle = handle;
            }
        }

        //private void ProcessExited(object sender, System.EventArgs e)
        //{
        //    Console.WriteLine("Exited");
        //}

        private string[] ParseFilePath(string command)
        {
            string s = command;
            //int idx = s.LastIndexOf('.');
            int idx = s.LastIndexOfAny(new char[] { '\\', '/' });

            //if (idx != -1) {
            //    Console.WriteLine(s.Substring(0, idx)); // "My. name. is Bond"
            //    Console.WriteLine(s.Substring(idx + 1)); // "_James Bond!"
            //}

            var arr = new string[2];
            arr[0] = command.Substring(0, idx);
            arr[1] = command.Substring(idx + 1);

            return arr;
        }

        private void SelectedItemSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selection = (Proc)this.ProcessListView.SelectedItem;

            var oldCommand = selection.Command.ToString();
            var oldName = selection.Name.ToString();

            var command = this.SelectedItemCommandTextBox.Text;
            var name = this.SelectedItemNickNameTextBox.Text;

            

            //Edit Command
            var indexOfCommand = Properties.Settings.Default.Processes.IndexOf(oldCommand);
            if(indexOfCommand > -1) {
                Properties.Settings.Default.Processes[indexOfCommand] = command;
            }
            //Edit Name
            var indexOfName = Properties.Settings.Default.Names.IndexOf(oldName);
            if(indexOfName > -1) {
                Properties.Settings.Default.Names[indexOfName] = name;
            }


            Properties.Settings.Default.Save();

            UpdateProcessList(this.ProcessListView.SelectedIndex);

        }

        private void SelectedItemDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selection = (Proc)this.ProcessListView.SelectedItem;

            var oldCommand = selection.Command.ToString();
            var oldName = selection.Name.ToString();

            //Remove Command
            var indexOfCommand = Properties.Settings.Default.Processes.IndexOf(oldCommand);
            if (indexOfCommand > -1) {
                Properties.Settings.Default.Processes.RemoveAt(indexOfCommand);
            }
            //Remove Name
            var indexOfName = Properties.Settings.Default.Names.IndexOf(oldName);
            if (indexOfName > -1) {
                Properties.Settings.Default.Names.RemoveAt(indexOfName);
            }

            Properties.Settings.Default.Save();
            UpdateProcessList();

        }

        private void StartAllButton_Click(object sender, RoutedEventArgs e)
        {
            var list = this.ProcessListView.Items;
            Console.WriteLine("click");
            foreach( Proc item in list) {
                if(item.WindowHandle == System.IntPtr.Zero) {
                    var split = item.Command.Split();

                    var parsePath = ParseFilePath(split[0]);
                    var arguments = item.Command.Remove(0, item.Command.IndexOf(' ') + 1);

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = parsePath[1];
                    startInfo.Arguments = arguments;
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = parsePath[0];

                    var process = Process.Start(startInfo);
                    process.WaitForInputIdle();
                    var handle = process.MainWindowHandle;

                    item.WindowHandle = handle;

                    System.Threading.Thread.Sleep(5000);
                }
            }
        }
    }
}

public class Proc
{
    public string Name { get; set; }

    public string Command { get; set; }

    public System.IntPtr WindowHandle { get; set; }
}