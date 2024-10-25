using System.Net.Sockets;
using System.Text;

namespace ConnectToServerTest;

using System.Net;

class Program
{
    static void Main(string[] args)
    {

        string url = "news.sunsite.dk";
        int port = 119;
        string username = "noahvi01@easv365.dk";
        string password = "4edf98";
        
        
        Connect(url, port, username, password);
        

    }

    private static void Connect(string url, int port, string username, string password)
    {
     
        try
        {
            using (TcpClient client = new TcpClient(url, port))
            {

                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream, Encoding.ASCII);
                StreamWriter writer = new StreamWriter(stream, Encoding.ASCII)
                {
                    
                    AutoFlush = true
                    
                };
                
                

                string response = reader.ReadLine();
                Console.WriteLine($"Server response: {response}\n");
                
                writer.WriteLine($"AUTHINFO USER {username}\r\n");
                Console.WriteLine($"Sent: AUTHINFO USER {username}");
                response = reader.ReadLine();
                Console.WriteLine($"Server response: {response}\n");
                


                if (response.StartsWith("381"))
                {
                    
                    writer.WriteLine($"AUTHINFO PASS {password}\r\n");
                    Console.WriteLine($"Sent: AUTHINFO PASS {password}\n");
                    
                    response = reader.ReadLine();
                    Console.WriteLine($"Server response: {response}");


                    if (response.StartsWith("281"))
                    {

                        Console.WriteLine("Successful Authentication");
                        
                        writer.WriteLine("LIST\r\n");
                        Console.WriteLine("LIST Sent");
                        
                        byte[] buffer = new byte[4096];
                        int bytesRead = 0;
                        MemoryStream memoryStream = new MemoryStream();
                        
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            memoryStream.Write(buffer, 0, bytesRead);
                            
                            string responsePart = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            Console.WriteLine(responsePart);
                        
                            if (responsePart.Contains("\r\n. \r\n"))
                            {
                                break;
                            }
                        }
                        byte[] responseBytes = memoryStream.ToArray();
                        Console.WriteLine("LIST command response: ");
                        
                        string listResponse = Encoding.ASCII.GetString(responseBytes);
                        Console.WriteLine($"List response:\n{listResponse}");

                    }


                }
                

            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        
    }



}