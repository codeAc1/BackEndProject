﻿$(document).ready(function () {
    $(document).on("click", ".deleteTag", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response =>
                {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data =>
                {
                    $(".tagTable").html(data);
                })
            }
        })

    })
    $(document).on("click", ".restoreTag", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Restore it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Restored!',
                            'Your file has been Restored.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {
                    $(".tagTable").html(data);
                })
            }
        })

    })

    $(document).on("click", ".deleteColor", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {
                    $(".ColorTable").html(data);
                })
            }
        })

    })
    $(document).on("click", ".restoreColor", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Restore it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Restored!',
                            'Your file has been Restored.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {
                    $(".ColorTable").html(data);
                })
            }
        })

    })

    $(document).on("click", ".deleteSize", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {
                    $(".SizeTable").html(data);
                })
            }
        })

    })
    $(document).on("click", ".restoreSize", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Restore it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Restored!',
                            'Your file has been Restored.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {

                    $(".SizeTable").html(data);
                })
            }
        })

    })

    $(document).on("click", ".deleteBrand", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {
                    $(".BrandTable").html(data);
                })
            }
        })

    })
    $(document).on("click", ".restoreBrand", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Restore it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Restored!',
                            'Your file has been Restored.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {

                    $(".BrandTable").html(data);
                })
            }
        })

    })
    $(document).on("click", ".deleteCategory", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {
                    $(".CategoryTable").html(data);
                })
            }
        })

    })
    $(document).on("click", ".restoreCategory", function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Restore it!'
        }).then((result) => {
            if (result.isConfirmed) {
                let url = $(this).attr("href");
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Restored!',
                            'Your file has been Restored.',
                            'success'
                        )
                    }

                    return response.text();
                }).then(data => {

                    $(".CategoryTable").html(data);
                })
            }
        })

    })

    $(document).on("click", "#deleteImage", function (e) {
        e.preventDefault();

        let url = $(this).attr("href");

        fetch(url).then(response => { return response.text() }).then(data => { $(".productupdate").html(data) });
    })
})