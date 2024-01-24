using System;
using System.Threading;
using LoadBalancer.Abstracts;
using LoadBalancer.Core;



class MonitorThread
{
    ManageableSession session;
    public MonitorThread(ManageableSession session)
    {
        this.session = session;
        Thread thread = new Thread(new ThreadStart(this.Run));
        thread.Start();
        Console.WriteLine("Monitor thread started");
    }


    private void Run()
    {
        try {
            while (true)
            {
                Check();
                Thread.Sleep(1000);
            }
        } catch(Exception e )
        {
            // Console.WriteLine(e);
        }
    }

    private void Check()
    {
        try {
            // Console.WriteLine("Monitor routine checks...");
            if (session.isHealthy())  
            {
                Console.WriteLine("SESSION IS HEALTHY");
                if (session.state.status() == Status.DOWN)
                {
                    Console.WriteLine("Session is health but downy, reconnecting...");
                    session.reconnect();
                }
                // Console.WriteLine("Session is healthy");
            } else {
                Console.WriteLine("SESSION IS UNHEALTHY");
                if (session.state.status() == Status.UP)
                {
                    Console.WriteLine("Session is unhealthy, marking as down...");
                    // session.Close();
                    session.state.nextState();
                }

                session.fix();
                // Console.WriteLine("Session is unhealthy");
            }

        } catch(Exception e) {
            
        }
    }
}