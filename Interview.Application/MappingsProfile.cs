using AutoMapper;
using Interview.Application.Core.Entitities;
using Interview.Application.Invoices;
using Interview.Application.Invoices.CreateInvoice;
using Interview.Application.Notes;
using Interview.Application.Notes.CreateNote;

namespace Interview.Application
{
    public class MappingsProfile: Profile
    {
        public MappingsProfile()
        {
            CreateMap<Invoice, InvoiceDto>();

            CreateMap<InvoiceDto, Invoice>();

            CreateMap<CreateInvoiceDto, Invoice>()
                .ForMember(dest =>
                    dest.Amount,
                    opt => opt.MapFrom(src => src.Amount.Value));

            CreateMap<Note, NoteDto>();

            CreateMap<NoteDto, Note>();

            CreateMap<CreateNoteDto, Note>()
                .ForMember(dest =>
                    dest.InvoiceId,
                    opt => opt.MapFrom(src => src.InvoiceId.Value));

        }
    }
}
