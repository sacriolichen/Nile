/*******************************************************************************\
                                  '                                                       //                                                                                              
                           '                                                              //  IIIIIIIIDDDDDDD                                                                     
                                     °                                                   //  I::::::ID::::::DDD                                                                  
                /¯¯¯¯\    °                                                              //  I::::::ID:::::::::DD                                                                
    '|\¯¯¯¯\/'/|       '|'  ____    °  /¯¯¯¯/|                                           //  II::::IIDDD:::DDD:::D                                                               
    '|;'|       '/'/       /| |\____\ '   '|       |;|             '/¯¯¯¯/|¯¯¯¯|          //    I::I    D:::D  D:::D     eeeeeeee       mmmm    mmmm      ooooooo   
     '/       /'/       /;'| /¯¯¯¯¯/|  '|\      '\|    '        |       |;|       |       //    I::I    D:::D   D:::D  ee::::::::ee   mm::::m  m::::mm  oo:::::::oo 
    /       /'/       /;;'/ '|        '|;|   '|;'\       \            |       |/____/|    //    I::I    D:::D   D:::D e::::eeeee:::eem:::::::mm:::::::mo:: ::::::::o
  '/       /'/       /;;'/'  '|\        \| '  '\;|       |/¯¯¯¯/|  |       |;|¯¯¯¯|       //    I::I    D:::D   D:::De::::e     e:::em::::::::::::::::mo:::ooooo:::o
  |____|;|____|;;/    '|;\_____\'   '/____/|____'|'|  |\____\|____|                       //    I::I    D:::D   D:::De:::::eeeee::::em:::mmm::::mmm:::mo::o     o::o
  |       |/|       |/       \;|         |   |       |;|       '|/' |;|       ||      '|  //    I::I    D:::D   D:::De:::::::::::::e m::m   m::m   m::mo::o     o::o
  |____| |____|         \|_____|    |____|/|____'| ° '\|____||____| '                    //    I::I    D:::D   D:::De::::eeeeeeeee  m::m   m::m   m::mo::o     o::o
                                     °                                                   //    I::I    D:::D  D:::D e:::::e         m::m   m::m   m::mo::o     o::o
                           '                                                              //  II::::IIDDD:::DDD:::D  e::::::e        m::m   m::m   m::mo:::ooooo:::o
                                           °                                             //  I::::::ID:::::::::DD    e::::::eeeeee  m::m   m::m   m::mo:::::::::::o
                     .                                                                    //  I::::::ID::::::DDD       ee:::::::::e  m::m   m::m   m::m oo:::::::oo 
                                            °                                            //  IIIIIIIIDDDDDDD            eeeeeeeeee  mmmm   mmmm   mmmm   ooooooo   
*********************************************************************************
*   File name:  IDemo.cs
*   Document no:
*   Document ver:
*   Design Responsible: 12234871
*   Description: [Description("Demo of specific interface, debugging usage")]
*   Date: 4/13/2020 09:46:21 AM
* 
*   COPYRIGHT (C) Update later                                                 *
\*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nile.Definition
{
    /// <summary>
    /// Demo interface
    /// </summary>
    public interface IArea : ICommonComponent
    {
        double Circle(double Radius);
        double Quadrate(double SideLength);
        double RegularTriangle(int sidelength);
        bool IsInitialized { get; }
    }
}
