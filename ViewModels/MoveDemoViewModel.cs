using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace TcADSNet_Demo.ViewModels
{
    public class MoveDemoViewModel : BindableBase
    {
        /// <summary>/// Prism Property/// </summary>
		/// <summary>/// Prism Property/// </summary>
		private static MoveDemoViewModel _instance;
        public static MoveDemoViewModel Instance
        {
            get { 
                if(_instance == null)
                {
                    _instance = new MoveDemoViewModel();
                }
                return _instance; 
            }
            set { _instance = value; }
        }


        private int _xCoord;
        public int XCoord
        {
            get { return _xCoord; }
            set { SetProperty(ref _xCoord, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private int _yCoord;
        public int YCoord
        {
            get { return _yCoord; }
            set { SetProperty(ref _yCoord, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private String _movePosition;
        public String MovePosition
        {
            get { return _movePosition; }
            set { SetProperty(ref _movePosition, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private float _velocity;
        public float Velocity
        {
            get { return _velocity; }
            set { SetProperty(ref _velocity, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private static bool _start;
        public static bool Start
        {
            get { return _start; }
            //set { SetProperty(ref _start, value); }
            set { _start = value; }
        }



        private int originY = 250;
        private int originX = 50;

        public MoveDemoViewModel()
        {
            MovePosition = "50,250,0,0";
        }

        public void SetPositionByNStep(int n)
        {
            String retSt = "";
            int xCoord = originX;
            int yCoord = originY;
            int dist = 2 * n;

            xCoord += dist;
            yCoord -= dist/2;

            retSt = xCoord.ToString() + "," + yCoord.ToString() + ",0,0";

            MovePosition = retSt;
            //Console.WriteLine(retSt);
        }

        public static void Oscilation()
        {
            float step = 0f;
            int dist = 0;
            int xCoord = MoveDemoViewModel.Instance.originX;
            int yCoord = MoveDemoViewModel.Instance.originY;
            bool direction = true;
            Console.WriteLine("Thread Start : {0}", MoveDemoViewModel.Start);
            while (MoveDemoViewModel.Start)
            {
                Console.WriteLine("Thread Running - X : {0}",xCoord);
                if (xCoord >= 500) direction = true;
                if (xCoord <= 50) direction = false;
                //if(xCoord > 60)
                //{
                //    direction = true;
                //}
                //else
                //{
                //    if(xCoord < 50)
                //    {
                //        direction = false;
                //    }
                //}
                //step = MainWindowViewModel.Instance.Velocity;
                step = 1f;
                Console.WriteLine("Direction|Step : {0}|{1}", direction, (int)step);
                if (direction)
                {
                    dist -= 1 * (int)step;
                    xCoord -= dist;
                    yCoord += dist / 2;
                }
                else
                {
                    dist += 1 * (int)step;
                    xCoord += dist;
                    yCoord -= dist / 2;
                }
                
                MoveDemoViewModel.Instance.MovePosition = xCoord.ToString() + "," + yCoord.ToString() + ",0,0";
                Console.WriteLine("{0} : {1}", MoveDemoViewModel.Instance.MovePosition, dist);
                //Thread.Sleep(500);
                Thread.Sleep((int)MoveDemoViewModel.Instance.Velocity);
            }
        }



    }/// Class
}/// Namespace
