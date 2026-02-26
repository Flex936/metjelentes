use std::fs::read_to_string;
use std::io::stdin;
mod weather;
use crate::weather::Weather;

fn read_file(filename: &str) -> Vec<String> {
    read_to_string(filename)
        .expect("Should have been able to read the file.")
        .lines()
        .map(String::from)
        .collect()
}

fn print_separator(exercise_number: u8) {
    println!("\n--- Exercise {} ---", exercise_number);
}

fn main() {
    // Loading data from file
    let mut weather_list: Vec<Weather> = Vec::new();

    for file_line in read_file("src/tavirathu13.txt") {
        let weather_entry = Weather::new(&file_line);
        weather_list.push(weather_entry);
    }

    println!("Successfully loaded {} records.", weather_list.len());

    // 2. exercise
    print_separator(2);
    print!("Enter settlement name: ");
    let mut settlement_input = String::new();
    stdin().read_line(&mut settlement_input).unwrap();
    let settlement_input = settlement_input.trim();

    // Find last weather entry for the settlement
    let last_weather = weather_list
        .iter()
        .rev()
        .find(|w| w.settlement == settlement_input);

    match last_weather {
        Some(weather) => {
            println!(
                "The last weather entry for the settlement was at {}:{}",
                weather.time.hour, weather.time.minute
            );
        }
        None => {
            println!("No weather entry found for {}.", settlement_input);
        }
    }

    // 3. exercise - Min/Max temperature
    print_separator(3);
    let min_temp = weather_list.iter().min_by_key(|w| w.temperature);
    let max_temp = weather_list.iter().max_by_key(|w| w.temperature);

    match min_temp {
        Some(weather) => {
            println!(
                "The minimum temperature was {} Celsius at {}:{}",
                weather.temperature, weather.time.hour, weather.time.minute
            );
        }
        None => {
            println!("No weather entry found for {}.", settlement_input);
        }
    }

    match max_temp {
        Some(weather) => {
            println!(
                "The maximum temperature was {} Celsius at {}:{}",
                weather.temperature, weather.time.hour, weather.time.minute
            );
        }
        None => {
            println!("No weather entry found for {}.", settlement_input);
        }
    }

    // 4. exercise - Wind quietness
    print_separator(4);
    let wind_quietness = weather_list.iter().filter(|w| w.wind_speed == "00");
    for wq in wind_quietness {
        println!(
            "Wind quietness at {}:{} in {}",
            wq.time.hour, wq.time.minute, wq.settlement
        );
    }

    // 5. exercise - Avg temp & temp difference
    print_separator(5);
    let avg_temp =
        weather_list.iter().map(|w| w.temperature).sum::<i8>() / weather_list.len() as i8;
    let temp_diff = weather_list.iter().map(|w| w.temperature).max().unwrap()
        - weather_list.iter().map(|w| w.temperature).min().unwrap();
    println!("Average temperature: {} Celsius", avg_temp);
    println!("Temperature difference: {} Celsius", temp_diff);
}
