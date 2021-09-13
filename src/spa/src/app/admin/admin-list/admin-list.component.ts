import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { EMPTY, merge, Observable, Subject } from 'rxjs';
import { concatMap, debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { AdminUserForListDto } from 'src/dto/models';
import { AdminEditComponent } from '../admin-edit/admin-edit.component';
import { AdminComponent } from '../admin/admin.component';
import { AdminService } from '../services/admin.service';

@Component({
  templateUrl: './admin-list.component.html',
  styleUrls: ['./admin-list.component.css']
})
export class AdminListComponent implements OnInit, OnDestroy, AfterViewInit {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource = new MatTableDataSource<AdminUserForListDto>();
  displayedColumns = ['firstName', 'lastName', 'email', 'role', 'actions'];
  paginationOptions = new Pagination();
  pagination!: Pagination;
  searchString = new FormControl('');

  constructor(
    private adminService: AdminService,
    private notify: NotificationService,
    public dialog: MatDialog,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(data => this.mapPagination(data.initData));

    this.searchString.valueChanges
      .pipe(
        takeUntil(this.unsubscribe),
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(() => this.getAdmins())
      )
      .subscribe(paginatedAdmins => this.mapPagination(paginatedAdmins));
  }

  ngAfterViewInit(): void {
    merge(this.paginator.page, this.sort.sortChange)
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.getAdmins())
      )
      .subscribe(paginatedAdmins => {
        this.mapPagination(paginatedAdmins);
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  private getDialogConfig(): MatDialogConfig<any> {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '600px';
    dialogConfig.autoFocus = false;

    return dialogConfig;
  }

  openAddDialog(): void {
    const dialogConfig = this.getDialogConfig();

    this.dialog
      .open(AdminComponent, dialogConfig)
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(returnUser => {
        if (returnUser) {
          this.dataSource.data.push(returnUser);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.pagination.totalItems++;
          this.dataSource._updateChangeSubscription();
        }
      });
  }

  openUpdateDialog(user: AdminUserForListDto): void {
    const dialogConfig = this.getDialogConfig();
    dialogConfig.data = user;
    this.dialog
      .open(AdminEditComponent, dialogConfig)
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(returnUser => {
        if (returnUser) {
          const updatedUser = this.dataSource.data.find(a => a.id === returnUser.id);
          if (updatedUser) {
            const index = this.dataSource.data.indexOf(updatedUser);
            this.dataSource.data[index].role.name = returnUser.role;
          }
        }
      });
  }

  deleteUser(user: AdminUserForListDto): void {
    console.log(user);

    this.notify
      .confirm('Are you sure you want to delete this user. !Note this cannot be undone')
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe),
        concatMap(response => {
          if (response) {
            return this.adminService.deleteUser(user.id);
          }

          return EMPTY;
        })
      )
      .subscribe(() => {
        this.dataSource.data.splice(
          this.dataSource.data.findIndex(x => x.id === user.id),
          1
        );
        this.pagination.totalItems--;
        this.dataSource._updateChangeSubscription();
        this.notify.success('User was deleted successfully');
      });
  }

  onSearchClear(): void {
    this.searchString.setValue('');
  }

  private getAdmins(): Observable<PaginatedResult<AdminUserForListDto[]>> {
    return this.adminService.getAdmins(
      this.paginator.pageIndex + 1,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction.toString(),
      this.searchString.value
    );
  }

  private mapPagination(result: PaginatedResult<AdminUserForListDto[]>): void {
    this.dataSource.data = result.result;
    this.pagination = result.pagination;
  }
}
