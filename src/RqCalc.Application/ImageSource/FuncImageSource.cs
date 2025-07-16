using RqCalc.Domain._Base;

namespace RqCalc.Application.ImageSource;

internal class FuncImageSource(Func<int, IImage?> getImage) : IImageSource
{
    public IImage? GetById(int id) => getImage(id);
}
