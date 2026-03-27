﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; // Potrebno da bismo koristili Stopwatch

namespace oblutkostein_3D
{
    public partial class Form1 : Form
    {
        int[] allTextures=               //all 32x32 textures
        {
         //Checkerboard
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,1,1,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,1,1,1,1,1,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,1,1,1,1,1,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,1,1,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,

         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0,

         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1,

         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 1,1,1,1,1,1,1,1, 0,0,0,0,0,0,0,0, 
         //Brick
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,

         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,

         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,

         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,
         //Window
         1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,    
               
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 
         1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 

         1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,   
               
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,  
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,
         1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 
         1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 
         //Door
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,  
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,  
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,    
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,    
         0,0,0,1,1,1,1,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,1,1,1,1,0,0,0,  
         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,  
         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,   
         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,     

         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,  
         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,    
         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,    
         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,   
         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,  
         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,  
         0,0,0,1,0,0,0,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,0,0,0,1,0,0,0,  
         0,0,0,1,1,1,1,1, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 1,1,1,1,1,0,0,0,  

         0,0,0,0,0,0,0,0, 0,0,0,0,0,1,0,1, 1,0,1,0,0,0,0,0, 0,0,0,0,0,0,0,0,  
         0,0,0,0,0,0,0,0, 0,0,1,1,1,1,0,1, 1,0,1,1,1,1,0,0, 0,0,0,0,0,0,0,0,   
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,    
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,    
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,  
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,  
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,   
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0, 
         
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,  
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,     
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,   
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,   
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,   
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,  
         0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1, 1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,   
         0,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,         
        };

        SolidBrush cetkaMiniMap = new SolidBrush(Color.Yellow);
        Pen olovkaDirection = new Pen(Color.Orange);
        Pen olovkaZid = new Pen(Color.Red, 8);

        double playerX = 300, playerY = 300;

        double rotationSpeed = 7.0;
        double speed = 155.0;

        // Igrac delta X, igrac delta Y (X i Y koraci igraca za odredjeni ugao), igrac Ugao (ugao u radijanima pod kojim igrac gleda)
        double playerdX, playerdY, playerA = 0.0;

        //Ray casting
        //X i Y koordinate na mapi, pozicija na mapi (map index), broj koraka koje ray pravi dok ne udari u zid
        int mx, my, mp, dof;
        //Trenutna X i Y pozicija vrha ray-a, ugao pod kojim ray putuje, offset/korak za koji se ray pomjera do sljedece minimap linije
        //te finalna udaljenost do najblizeg zida
        double rx, ry, ra, xoff, yoff, disT;

        //Stoperica koja pocinje brojati od nule cim je pokrenemo sa stopwatch.Start() - ekvivalentno milis u arduinu...
        Stopwatch stopwatch = new Stopwatch();

        //Pamti tacan trenutak kada se zavrsio prethodni frame
        //Uzima podatak iz stopwatch-a o tome koliko je tacno proslo od pokretanja programa do ovog trenutka
        //Izracunava koliko je vremena proteklo između dva frame-a
        double lastTime = 0, currentTime, deltaTime;

        bool goUp, goDown, goLeft, goRight;

        int mapX = 8, mapY = 8, mapS = 64;

        int[] mapW = 
        {
            1, 1, 1, 2, 1, 2, 1, 2,
            1, 0, 0, 2, 0, 0, 0, 1,
            1, 0, 0, 4, 0, 2, 0, 2,
            1, 2, 4, 2, 0, 0, 0, 1,
            3, 0, 0, 0, 0, 0, 0, 2,
            1, 0, 0, 0, 0, 1, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 2,
            1, 1, 1, 1, 1, 1, 1, 2,
        };

        public Form1()
        {
            InitializeComponent();

            // Sprecava treperenje tako sto prvo iscrta sve u memoriji, pa onda prikaze gotovu sliku (bez crtanja jednog po jednog elementa)
            this.DoubleBuffered = true;

            //Client size ne racuna title forme sto nam odgovara, za razliku od Size (512 + 1 zbog praznog mjesta na dnu minimape)
            this.ClientSize = new Size(1024, 513);

            stopwatch.Start();
            //Cim se zavrse sve druge naredbe i program bude u tzv. "Wait state-u" (Idle), pokrece se GameLoop funkcija 
            //Dakle ovim se izvlaci maksimum iz samog procesora racunara na kojem se pokrece igra
            Application.Idle += GameLoop;

            //Prvo racunanje deltaX i deltaY za pocetni ugao
            playerdX = Math.Cos(playerA);
            playerdY = Math.Sin(playerA);
        }

        private double distance(double ax, double ay, double bx, double by, double ang)
        {
            return (Math.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay)));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int xo, yo; // X i Y offset - dimenzije zidova

            //Nacrtaj minimapu
            for (int y = 0; y < mapY; y++)
            {
                for (int x = 0; x < mapX; x++)
                {
                    //Zbog memorijske produktivnosti umjesto 2D niza koristili smo 1D niz i pristupali elementima kao da je 2D niz preko formule: map[y * mapX + x]
                    if (mapW[y * mapX + x] > 0) { cetkaMiniMap.Color = Color.White; } else { cetkaMiniMap.Color = Color.Black; }
                    xo = x * mapS;
                    yo = y * mapS;

                    g.FillRectangle(cetkaMiniMap, xo + 1, yo + 1, mapS - 1, mapS - 1);
                }
            }

            //Nacrtaj igraca na minimapi
            cetkaMiniMap.Color = Color.Yellow;
            g.FillEllipse(cetkaMiniMap, (int)playerX, (int)playerY, 8, 8);

            //Nacrtaj liniju direkcije igraca na minimapi
            //centerX i centerY predstavljaju koordinate sredine naseg igraca na minimapi (odakle krece linija direkcije)
            int centerX = (int)playerX + 4;
            int centerY = (int)playerY + 4;
            // Mnozimo deltaX i deltaY sa 5 da dobijemo zeljenu duzinu linije direkcije
            g.DrawLine(olovkaDirection, centerX, centerY, (int)(centerX + playerdX * 5), (int)(centerY + playerdY * 5));

            //Nacrtaj ray
            ra = playerA - 0.0174533 * 30;
            if (ra < 0) ra += 2 * Math.PI;
            if (ra > 2 * Math.PI) ra -= 2 * Math.PI;

            for (int r = 0; r < 60; r++)
            {
                int vmt = 0; 
                int hmt = 0;

                //Provjeri horizontalne linije
                dof = 0;
                //Udaljenost od igraca do tacke gdje ray udara prvi horizontalni zid (na pocetku je 1000000 jer jos ne znamo gdje je zid)
                //X i Y koordinate gdje je ray udario horizontalni zid
                double disH = 1000000, hx = playerX, hy = playerY;

                double aTan = -1.0 / Math.Tan(ra);

                //Igrac gleda gore
                if (ra > Math.PI)
                {
                    //Pomocu trigonometrijske funkcije tangens pronalazimo koordinate najblize horizontalne linije, te X i Y korake do sljedece horizontalne linije
                    ry = ((int)(playerY / 64) * 64) - 0.0001;
                    rx = (playerY - ry) * aTan + playerX;
                    yoff = -64;
                    xoff = -yoff * aTan;
                }
                //Igrac gleda dole
                if (ra < Math.PI)
                {
                    //Pomocu trigonometrijske funkcije tangens pronalazimo koordinate najblize horizontalne linije, te X i Y korake do sljedece horizontalne linije
                    ry = ((int)(playerY / 64) * 64) + 64;
                    rx = (playerY - ry) * aTan + playerX;
                    yoff = 64;
                    xoff = -yoff * aTan;
                }
                //Igrac gleda ravno lijevo ili ravno desno (ray nikada ne pogadja horizontalnu liniju)
                if (ra == 0 || ra == Math.PI)
                {
                    rx = playerX;
                    ry = playerY;
                    dof = 8;
                }
                while (dof < 8)
                {
                    mx = (int)(rx / 64); // Dijelimo sa 64 da dobijemo kolonu 
                    my = (int)(ry / 64); // Dijelimo sa 64 da dobijemo red
                    mp = my * mapX + mx; // Pretvaramo 2D (red, kolona) u indeks za 1D niz map[]

                    if (mp > 0 && mp < mapX * mapY && mapW[mp] > 0) //Provjera da li je pogodjen horizontalni zid
                    {
                        hmt = mapW[mp];
                        //Pohranjujemo podatke o X i Y poziciji gdje je horizontalni zid pogodjen,
                        //te pomocu pitagorine teoreme izracunavamo udaljenost ray-a od igraca do tog pogodjenog horizontalnog zida
                        hx = rx;
                        hy = ry;
                        disH = distance(playerX, playerY, hx, hy, ra);
                        dof = 8; // Ray pogodio horizontalni zid - prekini provjeru za taj ray

                    }
                    else
                    {
                        // Sljedeca horizontalna linija...
                        rx += xoff;
                        ry += yoff;
                        dof++;
                    }
                }

                //Provjeri vertikalne linije
                dof = 0;
                //Udaljenost od igraca do tacke gdje ray udara prvi vertikalni zid (na pocetku je 1000000 jer jos ne znamo gdje je zid)
                //X i Y koordinate gdje je ray udario vertikalni zid
                double disV = 1000000, vx = playerX, vy = playerY;

                double nTan = -Math.Tan(ra);

                //Igrac gleda lijevo
                if (ra > Math.PI / 2 && ra < 3 * Math.PI / 2)
                {
                    //Pomocu trigonometrijske funkcije negativni tangens pronalazimo koordinate najblize vertikalne linije,
                    //te X i Y korake do sljedece vertikalne linije
                    rx = ((int)(playerX / 64) * 64) - 0.0001;
                    ry = (playerX - rx) * nTan + playerY;
                    xoff = -64;
                    yoff = -xoff * nTan;
                }
                //Igrac gleda desno
                if (ra < Math.PI / 2 || ra > 3 * Math.PI / 2)
                {
                    //Pomocu trigonometrijske funkcije negativni tangens pronalazimo koordinate najblize vertikalne linije,
                    //te X i Y korake do sljedece vertikalne linije
                    rx = ((int)(playerX / 64) * 64) + 64;
                    ry = (playerX - rx) * nTan + playerY;
                    xoff = 64;
                    yoff = -xoff * nTan;
                }
                //Igrac gleda gore ili dolje
                if (ra == 0 || ra == Math.PI)
                {
                    rx = playerX;
                    ry = playerY;
                    dof = 8;
                }
                while (dof < 8)
                {
                    mx = (int)(rx / 64); // Dijelimo sa 64 da dobijemo kolonu 
                    my = (int)(ry / 64); // Dijelimo sa 64 da dobijemo red
                    mp = my * mapX + mx; // Pretvaramo 2D (red, kolona) u indeks za 1D niz map[]

                    if (mp > 0 && mp < mapX * mapY && mapW[mp] > 0) //Provjera da li je pogodjen vertikalni zid
                    {
                        vmt = mapW[mp];
                        //Pohranjujemo podatke o X i Y poziciji gdje je vertikalni zid pogodjen,
                        //te pomocu pitagorine teoreme izracunavamo udaljenost ray-a od igraca do tog pogodjenog vertikalnog zida
                        vx = rx;
                        vy = ry;
                        disV = distance(playerX, playerY, vx, vy, ra);
                        dof = 8; // Ray pogodio vertikalni zid - prekini provjeru za taj ray
                    }
                    else
                    {
                        // Sljedeca vertikalna linija...
                        rx += xoff;
                        ry += yoff;
                        dof++;
                    }
                }
                //PROBLEMI

                double shade = 1.0;
                int hitWallType = 0;

                if (disV < disH) // Pogodjen prvo vertikalni zid
                {
                    shade = 0.5;
                    rx = vx;
                    ry = vy;
                    disT = disV;
                    hitWallType = vmt;
                    olovkaZid.Color = Color.FromArgb(230, 0, 0);
                }
                if (disH < disV) // Pogodjen prvo horizontalni zid
                {
                    rx = hx;
                    ry = hy;
                    disT = disH;
                    hitWallType = hmt;
                    olovkaZid.Color = Color.FromArgb(178, 0, 0);
                }
                olovkaDirection.Color = Color.Red;
                g.DrawLine(olovkaDirection, (int)centerX, (int)centerY, (int)rx, (int)ry);

                // Popravljanje "fish-eye" efekta
                double ca = playerA - ra;
                if (ca < 0) ca += 2 * Math.PI;
                if (ca > 2 * Math.PI) ca -= 2 * Math.PI;
                disT = disT * Math.Cos(ca);

                if (disT < 0.1) disT = 0.1;

                //Nacrtaj 3D zidove
                double lineH = (mapS * 320) / disT;
                double lineH_full = lineH;
                double lineOff = 160 - lineH / 2;
                if (lineH > 320) lineH = 320;
                if (lineOff < 0) lineOff = 0;

                //Odredjujemo koliko koraka u teksturi pravimo za svaki piksel na ekranu
                double ty_step = 32.0 / lineH_full;
                //Sluzi za vertikalno centriranje teksture kada je zid visi od ekrana ako je igrac preblizu, izracunavamo koliko redova teksture treba "odsjeći" 
                //sa vrha i dna kako bi sredina teksture ostala vidljiva na ekranu.
                double ty_off = 0.0;

                if (lineH_full > 320)
                {
                    // Ako je zid visi od ekrana, izracunaj koliko teksture treba preskociti (offset)
                    ty_off = (lineH_full - 320.0) / 2.0;
                }

                int y;

                //Pocetna Y pozicija u teksturi
                double ty = ty_off * ty_step;
                double tx;

                if (shade == 1)
                {
                    tx = (rx / 2.0) % 32;
                    if (ra > Math.PI) tx = 31 - tx;
                }
                else
                {
                    tx = (ry / 2.0) % 32;
                    if (ra > Math.PI / 2 && ra < 3 * Math.PI / 2) tx = 31 - tx;
                }

                int texOffset = (hitWallType - 1) * 1024;
                if (texOffset < 0) texOffset = 0;

                for (y = 0; y < lineH; y++)
                {
                    int ty_idx = (int)ty & 31;
                    int tx_idx = (int)tx & 31;

                    int pixelColor = allTextures[texOffset + (ty_idx * 32 + tx_idx)];
                    int bojaVal = (int)((pixelColor * 255) * shade);

                    olovkaZid.Color = Color.FromArgb(bojaVal, bojaVal, bojaVal);
                    g.DrawLine(olovkaZid, (int)(r * 8 + 530), (int)lineOff + y, (int)(r * 8 + 530), (int)lineOff + y + 1);

                    ty += ty_step;
                }

                ra += 0.0174533;
                if (ra < 0) ra += 2 * Math.PI;
                if (ra > 2 * Math.PI) ra -= 2 * Math.PI;
            }

        }

        private void GameLoop(object sender, EventArgs e)
        {
            //Uvodjenjem ovih varijabli rjesavamo problem veceg i manjeg FPS-a, da se igrac koji ima bolje performanse racunara ne bi kretao brze u odnosu na
            //onog koji ima losije, uvodimo deltaTime koji ce za veci FPS biti znatno manji nego za manji FPS, pa ce se oba igraca kretati jednakom brzinom
            currentTime = stopwatch.Elapsed.TotalSeconds;
            deltaTime = currentTime - lastTime;
            lastTime = currentTime;

            UpdateGame(deltaTime);

            // Proglasava trenutnu sliku starom i ponovo poziva dogadjaj Paint
            this.Invalidate();
        }

        private void UpdateGame(double dt)
        {

            if (!(goUp && goDown))
            {
                //Rjesavamo problem prolazenja igraca kroz zidove tako sto provjeravamo da li je zid ispred ili iza njega za odredjenu vrijednost offseta
                //Uvodimo dvije offset vrijednosti jer se kolizija moze desiti i sa horizontalnim i sa vertikalnim zidom
                int xo = 0;
                int yo = 0;

                //Ako se igrac krece udesno (playerdX > 0) provjerava se vertikalni zid cija je X pozicija za 20 veca od pozicije igraca
                //Ako se igrac krece ulijevo (playerdX < 0) provjerava se vertikalni zid cija je X pozicija za 20 manja od pozicije igraca
                if (playerdX < 0) { xo = -20; } else { xo = 20; }

                //Ako se igrac krece prema gore (playerdY < 0) provjerava se horizontalni zid cija je Y pozicija za 20 manja od pozicije igraca
                //Ako se igrac krece prema dole (playerdY > 0) provjerava se horizontalni zid cija je Y pozicija za 20 veca od pozicije igraca
                if (playerdY < 0) { yo = -20; } else { yo = 20; }

                int ipx = (int)(playerX / 64.0); // Trenutna kolona na kojoj se nalazi igrac
                int ipx_add_xo = (int)((playerX + xo) / 64.0); // Kolona ispred igraca (pri kretanju naprijed / lijevo-desno)
                int ipx_sub_xo = (int)((playerX - xo) / 64.0); // Kolona iza igraca (pri kretanju nazad / lijevo-desno)

                int ipy = (int)(playerY / 64.0); // Trenutni red u kojem se nalazi igrac
                int ipy_add_yo = (int)((playerY + yo) / 64.0); // Red ispred igraca (pri kretanju naprijed / gore-dole)
                int ipy_sub_yo = (int)((playerY - yo) / 64.0); // Red iza igraca (pri kretanju nazad / gore-dole)

                if (goUp)
                {
                    if (mapW[ipy * mapX + ipx_add_xo] == 0) { playerX += playerdX * speed * dt; } //Sudar sa vertikalnim zidom - kretanje naprijed
                    if (mapW[ipy_add_yo * mapX + ipx] == 0) { playerY += playerdY * speed * dt; } //Sudar sa horizontalnim zidom - kretanje naprijed
                }
                if (goDown)
                {
                    if (mapW[ipy * mapX + ipx_sub_xo] == 0) { playerX -= playerdX * speed * dt; } // Sudar sa vertikalnim zidom - kretanje nazad
                    if (mapW[ipy_sub_yo * mapX + ipx] == 0) { playerY -= playerdY * speed * dt; } // Sudar sa horizontalnim zidom - kretanje nazad
                }
            }
            //Za kretanje igraca naprijed/nazad ne dodajemo vise fiksan korak zato sto igrac ne mora gledati pod uglom od 90 stepeni
            //Zato racunamo korake deltaX i deltaY koji se racunaju u zavisnosti od ugla pod kojim igrac gleda (u radijanima)
            if (!(goLeft && goRight))
            {
                if (goLeft)
                {
                    playerA -= rotationSpeed * dt;
                    if (playerA < 0) playerA += 2.0 * Math.PI;
                    playerdX = Math.Cos(playerA);
                    playerdY = Math.Sin(playerA);
                }

                if (goRight)
                {
                    playerA += rotationSpeed * dt;
                    if (playerA > 2.0 * Math.PI) playerA -= 2.0 * Math.PI;
                    playerdX = Math.Cos(playerA);
                    playerdY = Math.Sin(playerA);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) goUp = true;
            if (e.KeyCode == Keys.S) goDown = true;
            if (e.KeyCode == Keys.A) goLeft = true;
            if (e.KeyCode == Keys.D) goRight = true;
            if (e.KeyCode == Keys.E)
            {
                int xo = 0;
                if (playerdX < 0) { xo = -25; } else { xo = 25; }
                int yo = 0;
                if (playerdY < 0) { yo = -25; } else { yo = 25; }

                int ipx = (int)(playerX / 64.0); // Trenutna kolona na kojoj se nalazi igrac
                int ipx_add_xo = (int)((playerX + xo) / 64.0); // Kolona ispred igraca (pri kretanju naprijed / lijevo-desno)
                
                int ipy = (int)(playerY / 64.0); // Trenutni red u kojem se nalazi igrac
                int ipy_add_yo = (int)((playerY + yo) / 64.0); // Red ispred igraca (pri kretanju naprijed / gore-dole)
                if (mapW[ipy_add_yo * mapX + ipx_add_xo] == 4) { mapW[ipy_add_yo * mapX + ipx_add_xo] = 0; }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) goUp = false;
            if (e.KeyCode == Keys.S) goDown = false;
            if (e.KeyCode == Keys.A) goLeft = false;
            if (e.KeyCode == Keys.D) goRight = false;
        }
    }
}