#include <windows.h>

extern "C" __declspec(dllexport) int __stdcall Summa (int a, int b)
{
    return a+b;
}

extern "C" __declspec(dllexport) int __stdcall Sub (int a, int b)
{
    return a-b;
}

extern "C" __declspec(dllexport) int __stdcall Mult (int a, int b)
{
    return a*b;
}

extern "C" __declspec(dllexport) int __stdcall Division (int a, int b)
{
    return a/b;
}