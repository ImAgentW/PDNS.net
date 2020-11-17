"use strict";

function refreshPing() {
	let cardBody = $('#server-status');
	$(cardBody).empty();
	$.get('/ServerStatus', function (data) {
		$(cardBody).html(data);
	}).fail(function () {
		//return null;
	});
}

$(document).ready(function () {
	//Notify
	$.notify({
		icon: 'flaticon-alarm-1',
		title: 'PDNS.net Atlantis UI',
		message: 'ASP.net Based Dashboard for PowerDNS',
	}, {
		type: 'secondary',
		placement: {
			from: "bottom",
			align: "right"
		},
		timer: 3000
	});

	// JQVmap
	$.get("./Map", function (data) {
		let regions = new Array;
		$('#map-info').html(data);
		$('#map-info table td:first-child').each(function () {
			if ($(this).text() != 'private') {
				regions.push($(this).text());
			}
		});

		$('#map-vector').vectorMap(
			{
				map: 'world_en',
				backgroundColor: 'transparent',
				borderColor: '#fff',
				borderWidth: 2,
				color: '#e4e4e4',
				enableZoom: true,
				hoverColor: '#35cd3a',
				hoverOpacity: null,
				normalizeFunction: 'linear',
				scaleColors: ['#b6d6ff', '#005ace'],
				selectedColor: '#35cd3a',
				selectedRegions: regions,
				showTooltip: true,
				onRegionClick: function (element, code, region) {
					return false;
				}
			}
		);
	});

	$("#activeUsersChart").sparkline([112, 109, 120, 107, 110, 85, 87, 90, 102, 109, 120, 99, 110, 85, 87, 94], {
		type: 'bar',
		height: '100',
		barWidth: 9,
		barSpacing: 10,
		barColor: 'rgba(255,255,255,.3)'
	});

	$(".btn-close-card").on("click", function () {
		var e = $(this).parents(".row-card-no-pd");
		e.remove();
	});

	$('#birthday').datepicker({
		format: 'dd/mm/yyyy'
	});

	$('form').submit((e) => {
		e.preventDefault();
		let messageContent = document.createElement("pre");
		let formData = new FormData(e.target);
		$.ajax({
			url: $(e.target).attr('action'),
			type: $(e.target).attr('method'),
			data: formData,
			processData: false,
			contentType: false,
			success: function (data) {
				messageContent.innerHTML = data['Message'];
				swal({
					title: data['Title'],
					content: messageContent,
					icon: data['Icon'],
					timer: 10000,
					buttons: false
				});
			}
		}).fail(function (e) {
			let data = e.responseJSON;
			messageContent.innerHTML = data['Message'];
			swal({
				title: data['Title'],
				content: messageContent,
				icon: data['Icon'],
				buttons: {
					confirm: {
						className: 'btn btn-success'
					}
				}
			});
		});
	});

	refreshPing();
});
