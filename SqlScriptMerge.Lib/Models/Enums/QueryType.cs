namespace SqlScriptMerge.Lib.Models.Enums;

internal enum QueryType
{
    NotRecognized,
    Comment,

    Use,

    CreateTable,
    AlterTable,
    DropTable,

    InsertData,
    DeleteData,
    UpdateData,

    SelectData,

    CreateIndex,
    DropIndex,

    CreateView,
    DropView,
}
