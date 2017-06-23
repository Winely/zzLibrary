// Penalty.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include <iostream>
#include "Penalty.h"
using namespace std;

int __stdcall test01(int a, int b, int c) {
	return a + b + c;
}
int __stdcall test02(int a, int b) {
	return a - b;
}

double __stdcall penalty(double time) {
	double penalty=0;
	if (time >7 && time<=365) {
		penalty = time*0.05;
	}
	else if (time > 365) {
		penalty = 365*0.05;
	}
	else
	{
		penalty = 0;
	}
	return penalty;
	
}
