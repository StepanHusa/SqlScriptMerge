namespace SqlScriptMerge.Lib.Models.Enums;

public enum QueryType
{
    NotRecognized,
    Comment,

    Use,

    CreateTable,
    AlterTable,
    RenameTable,
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
