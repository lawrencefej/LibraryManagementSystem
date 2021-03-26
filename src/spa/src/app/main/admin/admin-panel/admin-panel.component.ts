import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';

import { ActivatedRoute } from '@angular/router';
import { AdminComponent } from '../admin/admin.component';
import { AdminService } from 'src/app/_services/admin.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'src/app/_services/notification.service';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  users: User[];
  dataSource: MatTableDataSource<User>;
  displayedColumns = ['firstName', 'lastName', 'email', 'role', 'actions'];
  paginationOptions = new Pagination();
  // Set to true to fix pagination issue.
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private adminService: AdminService,
    private notify: NotificationService,
    public dialog: MatDialog,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data.admins;
      this.dataSource = new MatTableDataSource<User>(this.users);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  private getDialogConfig() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '440px';

    return dialogConfig;
  }

  openAddAssetDialog() {
    const dialogConfig = this.getDialogConfig();

    this.dialog
      .open(AdminComponent, dialogConfig)
      .afterClosed()
      .subscribe(data => {
        if (data) {
          this.users.push(data);
          this.dataSource = new MatTableDataSource<User>(this.users);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        }
      });
  }

  public updateAsset(element: any) {
    const dialogConfig = this.getDialogConfig();
    dialogConfig.data = element;
    this.dialog
      .open(AdminComponent, dialogConfig)
      .afterClosed()
      .subscribe(data => {
        if (data) {
          const user = this.users.find(a => a.id === data.id);
          const index = this.users.indexOf(user);
          this.users[index] = data;
          this.dataSource = new MatTableDataSource<User>(this.users);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        }
      });
  }

  deleteUser(user: User) {
    this.notify
      .confirm('Are you sure you want to delete this user. !Note this cannot be undone')
      .afterClosed()
      .subscribe(response => {
        if (response) {
          this.adminService.deleteUser(user.id).subscribe(
            () => {
              this.users.splice(this.users.findIndex(x => x.id === user.id), 1);
              this.dataSource = new MatTableDataSource<User>(this.users);
              this.dataSource.paginator = this.paginator;
              this.dataSource.sort = this.sort;
              this.notify.success('User was deleted successfully');
            },
            error => {
              this.notify.error(error);
            }
          );
        }
      });
  }
}
