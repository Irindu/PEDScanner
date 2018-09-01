// TestDll1.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "TestDll1.h"


// This is an example of an exported variable
TESTDLL1_API int nTestDll1=0;

// This is an example of an exported function.
TESTDLL1_API void HelloDll1()
{
	std::cout << "Hello from dll1 \n";
}

