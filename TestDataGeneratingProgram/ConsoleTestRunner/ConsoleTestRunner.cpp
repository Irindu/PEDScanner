// ConsoleTestRunner.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "..\TestDll1\TestDll1.h"

int main()
{
	std::cout << "Starting Test Application.\n";
	HelloDll1();
	std::cout << "Test Application Ended. Press any key to continue.\n";
	std::cin.get();
    return 0;
}

