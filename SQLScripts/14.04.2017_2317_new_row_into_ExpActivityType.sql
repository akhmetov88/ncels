insert into Dictionaries (Id, Type, Code, Name, NameKz, DisplayName, ParentId, IsGuide)
values(NEWID(), 'ExpActivityType', '5', N'Согласование итогового документа', N'Согласование итогового документа', N'Согласование итогового документа',
(select Id from Dictionaries where Type='ExpAgreedDocType' and Code='5'), 0)