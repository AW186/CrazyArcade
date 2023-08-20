use std::vec::Vec;
pub trait ISerializable {
    fn serialize(&self) -> Vec<u8>;
    fn get_type(&self) -> u8;
    fn get_id(&self) -> u8;
    fn set_id(&mut self, id: u8);
}