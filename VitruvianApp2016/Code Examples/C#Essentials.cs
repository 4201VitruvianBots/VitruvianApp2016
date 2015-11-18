using System;

namespace VitruvianApp2016
{
	public class C_Essentials
	{
		//Data types
		int varA;							// int = interger value, all numbers are solid (i.e. 1,2,3, not 2.5 or 3.2)
		int varB = 100;					
		double varC;						// double, its value can be a decimal (i.e. 2.1, 4.3)
		const double varD = 3.14159; 		// const = constant, its value cannot be changed elsewhere
		uint varE;							// unsigned int: value is always positive. Typically, in other programs, this will be written as unsigned <data type>.
		int varF = varB/(int)varD;			// (type): casts to a certain type to the value/variable to the right (i.e. (int)varD make the value 3, useful for certain circumstances)
		string varG = "text";				// string, text values
		bool varH;							// only true or false
		enum Alphabet{a,b,c};				// enum = enumerated values, all variables within the brackets have a numberical value assocaited with them

		int[] arrayA = new int[50];			//array of intergers, must have a defined value for how large the array is. Useful for making large numbers of variables o use/store data.

		public C_Essentials ()
		{	
			int mathA = varB + varC;		// Adding intergers
			int mathB = 10;
			mathB += varB;					// Add itself and varB and set it to mathB (mathB=10+100=110)
			int varI = 3;
			int mathC = varB % varI;		//% = modulus, returns the remainder of a division (i.e. 100%3 = 1)

			//for loops
			int N=20;
			for (int i = 0; i < N; i++) {	// Create an int i to a starting value; so long as i is less than N, continue the loop; after each iteration of the loop, increment i by 1
				//Repeated code goes here, loop will repeat for 20 times
			}
			int count = 0;
			bool test = true;
			while (test) {
				//Repeated code goes here, loop will loop indefinately (Can be very bad). Always have some thing to get you out of a while loop
				count++; // ++ to the end of an interger is a post-increment value - count will increase by 1 every time this loops
				if (count == 20) {			// if() stantement, if the test inside of the parenthesis is true, run the if statement, otherwise, assume it is false
					test = false;			// mkes the test false for the loop to end
				}
			}
		}
	}
}

