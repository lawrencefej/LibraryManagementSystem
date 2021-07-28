import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { merge } from 'rxjs';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { MemberService } from 'src/app/_services/member.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { MemberComponent } from '../member/member.component';

@Component({
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements AfterViewInit, OnInit {
  members: User[] = [];
  pagination: Pagination;
  dataSource = new MatTableDataSource<User>(this.members);
  searchString = '';
  displayedColumns = ['libraryCardNumber', 'firstName', 'lastName', 'email', 'actions'];
  paginationOptions = new Pagination();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private memberService: MemberService,
    private route: ActivatedRoute,
    private notify: NotificationService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.pagination = data.members.pagination;
      this.members = data.members.result;
      this.dataSource = new MatTableDataSource<User>(this.members);
    });
  }

  ngAfterViewInit(): void {
    merge(this.paginator.page, this.sort.sortChange).subscribe(() => {
      this.loadData();
    });
  }

  filterList(): void {
    this.searchString.trim().toLocaleLowerCase();
    this.loadData();
  }

  onSearchClear(): void {
    this.searchString = '';
    this.filterList();
  }

  private getDialogConfig(): MatDialogConfig<any> {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';

    return dialogConfig;
  }

  updateMember(element: any): void {
    const dialogConfig = this.getDialogConfig();
    dialogConfig.data = element;
    this.dialog.open(MemberComponent, dialogConfig);
  }

  openAddMemberDialog(): void {
    const dialogConfig = this.getDialogConfig();

    this.dialog.open(MemberComponent, dialogConfig);
  }

  deleteAsset(member: User): void {
    this.notify
      .confirm('Are you sure you sure you want to delete this member')
      .afterClosed()
      .subscribe(res => {
        if (res) {
          this.memberService.deleteMember(member.id).subscribe(
            () => {
              this.members.splice(
                this.members.findIndex(x => x.id === member.id),
                1
              );
              this.notify.success('Member was deleted successfully');
              this.pagination.totalItems--;
              this.dataSource = new MatTableDataSource<User>(this.members);
            },
            error => {
              this.notify.error(error);
            }
          );
        }
      });
  }

  loadData(): void {
    this.memberService
      .getPaginatedMembers(
        this.paginator.pageIndex + 1,
        this.paginator.pageSize,
        this.sort.active,
        this.sort.direction.toString(),
        this.searchString
      )
      .subscribe(
        (res: PaginatedResult<User[]>) => {
          this.members = res.result;
          this.dataSource = new MatTableDataSource<User>(this.members);
        },
        error => {
          this.notify.error(error);
        }
      );
  }
}
