document.addEventListener("DOMContentLoaded", function () {
    const patientIdField = document.getElementById("patientId");
    const patientName = document.getElementById("patientName");
    const patientNameDisplay = document.getElementById("patientNameDisplay");
    const staffCategoryDropdown = document.getElementById("staffCategory");
    const staffMemberDropdown = document.getElementById("staffMember");
    const staffMemberNameHidden = document.getElementById("staffMemberName");

    patientIdField.addEventListener("blur", function () {
        const personnummer = patientIdField.value.trim();
        if (!personnummer) return;

        fetch(`/Bokning/GetPatientInfo?personnummer=${personnummer}`)
            .then(response => response.json())
            .then(data => {
                if (data && data.name) {
                    patientName.value = data.name;
                    patientNameDisplay.value = data.name;
                } else {
                    alert("Ingen patient hittades med detta personnummer.");
                    patientName.value = "";
                    patientNameDisplay.value = "";
                }
            });
    });

    staffCategoryDropdown.addEventListener("change", function () {
        const category = staffCategoryDropdown.value;
        if (!category) {
            staffMemberDropdown.innerHTML = `<option value="">-- Välj vårdgivare --</option>`;
            return;
        }

        fetch(`/Bokning/GetStaffMembers?category=${category}`)
            .then(response => response.json())
            .then(data => {
                staffMemberDropdown.innerHTML = `<option value="">-- Välj vårdgivare --</option>`;
                data.forEach(staff => {
                    staffMemberDropdown.innerHTML += `<option value="${staff.staffMemberId}" data-name="${staff.name}">${staff.name}</option>`;
                });
            });
    });

    staffMemberDropdown.addEventListener("change", function () {
        const selectedOption = staffMemberDropdown.options[staffMemberDropdown.selectedIndex];
        staffMemberNameHidden.value = selectedOption.getAttribute("data-name");
    });
});



// FAQ klickbar funktionalitet

    document.querySelectorAll('.faq-question').forEach(item => {
        item.addEventListener('click', () => {
            let parent = item.parentNode;
            parent.classList.toggle('active'); // Lägg till/tar bort klassen "active"
            let answer = parent.querySelector('.faq-answer');

            // Visa eller dölj svaret
            if (answer.style.display === "block") {
                answer.style.display = "none";
            } else {
                answer.style.display = "block";
            }
        });
    });

function toggleView() {
    const bookingSection = document.getElementById("bookingSection");
    const bookedSection = document.getElementById("bookedSection");
    const toggleButton = document.getElementById("toggleButton");
    const backButton = document.getElementById("backButton");

    bookingSection.classList.toggle("hidden");
    bookedSection.classList.toggle("hidden");
    toggleButton.classList.toggle("hidden");
    backButton.classList.toggle("hidden");
}
