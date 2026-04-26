using System;
namespace Quadratic_equation_original;

public static class EquationSolver
{
    //посчитаем и воспользуемся кортежем для возвращения двух корней
    public static (int count ,double x1, double x2) QuadraticEquationSolver(double a,double b,double c)
    {
        //если а=0 -> линейное уранение
        if (a==0)
        {
            if (b==0) return (0,double.NaN,double.NaN);

            double x=-c/b;
            return(1,x,double.NaN);
        }

        //квадратное уравнение

        double discriminant = b * b - 4 * a * c;

        if(discriminant<0) return (0,double.NaN,double.NaN);
        if (discriminant==0) return (1,-b/(2*a),double.NaN);


        double x1 = (-b+Math.Sqrt(discriminant))/(2*a);
        double x2 = (-b-Math.Sqrt(discriminant))/(2*a);

        return (2,x1, x2);
    }
}