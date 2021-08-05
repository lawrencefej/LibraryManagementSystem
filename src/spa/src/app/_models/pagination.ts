export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
  pageSizeOptions: number[];
}

export class PaginatedResult<T> {
  // TODO add initializer
  result!: T;
  pagination!: Pagination;
}

export class Pagination implements Pagination {
  pageSizeOptions: number[] = [10, 5, 15, 20];
}
