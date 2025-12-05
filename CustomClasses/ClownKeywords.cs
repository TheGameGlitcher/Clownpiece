using LBoLEntitySideloader.CustomKeywords;
using Clownpiece.Status;

namespace Clownpiece.CustomClasses
{
    public static class ClownKeyword
    {
        public static CardKeyword TorchLinked = new CardKeyword(nameof(DummyTorchLinkedSe)) { descPos = KwDescPos.First };
        public static CardKeyword TorchLinkedFairy = new CardKeyword(nameof(DummyTorchLinkedFairySe)) { descPos = KwDescPos.First };
    }
}