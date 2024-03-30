using System.Numerics;

namespace PGMQ.NET.Models;

public class PGMQModel<T>
{
    public BigInteger MessageId { get; init; }
    public T? Message { get; init; }
    public int ReadCount { get; set; }
    public DateTime EnqueueAt { get; set; }
    public DateTime Vt { get; set; }

    internal PGMQModel()
    {

    }
    public static implicit operator T(PGMQModel<T> model)
    {
        return model.Message!;
    }
}
