/**
 * Dark Mode Toggle Script
 * Handles switching between light and dark themes
 */

// Initialize dark mode on page load
document.addEventListener('DOMContentLoaded', function() {
    // Check if dark mode preference is saved
    const isDarkMode = localStorage.getItem('darkMode') === 'true';
    
    if (isDarkMode) {
        enableDarkMode();
    }

    // Add event listener to dark mode toggle button
    const darkModeToggle = document.getElementById('darkModeToggle');
    if (darkModeToggle) {
        darkModeToggle.addEventListener('click', toggleDarkMode);
    }
});

/**
 * Toggle dark mode on and off
 */
function toggleDarkMode() {
    const body = document.body;
    
    if (body.classList.contains('dark-mode')) {
        disableDarkMode();
    } else {
        enableDarkMode();
    }
}

/**
 * Enable dark mode by adding dark-mode class and saving preference
 */
function enableDarkMode() {
    document.body.classList.add('dark-mode');
    localStorage.setItem('darkMode', 'true');
    updateDarkModeButton(true);
}

/**
 * Disable dark mode by removing dark-mode class and saving preference
 */
function disableDarkMode() {
    document.body.classList.remove('dark-mode');
    localStorage.setItem('darkMode', 'false');
    updateDarkModeButton(false);
}

/**
 * Update the dark mode button text based on current mode
 * @param {boolean} isDarkMode - Whether dark mode is currently enabled
 */
function updateDarkModeButton(isDarkMode) {
    const button = document.getElementById('darkModeToggle');
    if (button) {
        button.textContent = isDarkMode ? '☀️ Light' : '🌙 Dark';
    }
}
