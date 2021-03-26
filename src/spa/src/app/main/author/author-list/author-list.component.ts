import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';

import { ActivatedRoute } from '@angular/router';
import { Author } from 'src/app/_models/author';
import { AuthorComponent } from '../author/author.component';
import { AuthorService } from 'src/app/_services/author.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'src/app/_services/notification.service';
import { merge } from 'rxjs';

@Component({
  selector: 'app-author-list',
  templateUrl: './author-list.component.html',
  styleUrls: ['./author-list.component.css'],
})
export class AuthorListComponent implements AfterViewInit, OnInit {
  authors: Author[] = [];
  pagination: Pagination;
  dataSource = new MatTableDataSource<Author>(this.authors);
  searchString = '';
  displayedColumns = ['firstName', 'lastName', 'actions'];
  paginationOptions = new Pagination();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private authorService: AuthorService,
    private route: ActivatedRoute,
    private notify: NotificationService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.pagination = data.authors.pagination;
      this.authors = data.authors.result;
      this.dataSource = new MatTableDataSource<Author>(this.authors);
    });
  }

  ngAfterViewInit() {
    merge(this.paginator.page, this.sort.sortChange).subscribe(() => {
      this.loadData();
    });
  }

  filterList() {
    this.searchString.trim().toLocaleLowerCase();
    this.loadData();
  }

  onSearchClear() {
    this.searchString = '';
    this.filterList();
  }

  private getDialogConfig() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '440px';

    return dialogConfig;
  }

  updateAuthor(element: any) {
    const dialogConfig = this.getDialogConfig();
    dialogConfig.data = element;
    this.dialog.open(AuthorComponent, dialogConfig);
  }

  openAddAuthorDialog() {
    const dialogConfig = this.getDialogConfig();

    this.dialog.open(AuthorComponent, dialogConfig);
  }

  loadData() {
    this.authorService
      .getPaginatedAuthors(
        this.paginator.pageIndex + 1,
        this.paginator.pageSize,
        this.sort.active,
        this.sort.direction.toString(),
        this.searchString
      )
      .subscribe(
        (res: PaginatedResult<Author[]>) => {
          this.authors = res.result;
          this.dataSource = new MatTableDataSource<Author>(this.authors);
        },
        (error) => {
          this.notify.error(error);
        }
      );
  }
}
