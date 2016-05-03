// TTAPI2013.cpp : main project file.
#include "stdafx.h"
#include "TTAPIFunctions.h"

using namespace System;
using namespace TradingTechnologies;

int main(array<System::String ^> ^args)
{
	TTAPIFunctions ^ tf = gcnew TTAPIFunctions("ZACK", "12345678");
	tf->Start();
    return 0;
}