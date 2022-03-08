﻿namespace WebApi_test.DTO
{
    public class FilterMovieDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;
        public PaginationDTO Pagination
        { get {
                return new PaginationDTO() { Page = Page, RecordPerPage = RecordsPerPage };
        }
        }
        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool InTheaters { get; set; }
        public bool UpComingReleases { get; set; }
        public string OrderingField { get; set; }
        public bool AscendingOrder { get; set; } = true;


    } }
