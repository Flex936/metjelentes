pub struct Time {
    pub hour: u8,
    pub minute: u8,
}

pub struct Weather {
    pub settlement: String,
    pub time: Time,
    pub wind_direction: String,
    pub wind_speed: String,
    pub temperature: i8,
}

impl Weather {
    pub fn new(line: &str) -> Self {
        let data: Vec<&str> = line.split_whitespace().collect();
        
        let time_str = data[1];
        let hour = time_str[0..2].parse().unwrap_or(0);
        let minute = time_str[2..4].parse().unwrap_or(0);

        Weather {
            settlement: String::from(data[0]),
            time: Time { hour, minute },
            wind_direction: data[2][0..3].to_string(),
            wind_speed: data[2][3..5].to_string(),
            temperature: data[3].parse().unwrap_or(0),
        }
    }
}