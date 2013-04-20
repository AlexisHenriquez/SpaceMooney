using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Buyable
{
    public int Cost { get; set; }

    public void Comprar<T>(Player player)
            where T :  Buildable, new()
    {
        T machine = new T(); 

        if (player.Budget >= machine.Buyable.Cost)
        {
            player.Budget -= machine.Buyable.Cost;
        }
        else
        {
            throw new InsuficientBudgetException("No hay suficiente presupuesto para realizar esta operación");
        }

    }
}